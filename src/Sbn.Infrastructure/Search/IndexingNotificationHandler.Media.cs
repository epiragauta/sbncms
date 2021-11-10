using System;
using System.Collections.Generic;
using Sbn.Cms.Core.Cache;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Notifications;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Services.Changes;
using Sbn.Cms.Core.Sync;
using Sbn.Extensions;

namespace Sbn.Cms.Infrastructure.Search
{
    public sealed class MediaIndexingNotificationHandler : INotificationHandler<MediaCacheRefresherNotification>
    {
        private readonly ISbnIndexingHandler _sbnIndexingHandler;
        private readonly IMediaService _mediaService;

        public MediaIndexingNotificationHandler(ISbnIndexingHandler sbnIndexingHandler, IMediaService mediaService)
        {
            _sbnIndexingHandler = sbnIndexingHandler ?? throw new ArgumentNullException(nameof(sbnIndexingHandler));
            _mediaService = mediaService ?? throw new ArgumentNullException(nameof(mediaService));
        }

        public void Handle(MediaCacheRefresherNotification args)
        {
            if (!_sbnIndexingHandler.Enabled)
            {
                return;
            }

            if (Suspendable.ExamineEvents.CanIndex == false)
            {
                return;
            }

            if (args.MessageType != MessageType.RefreshByPayload)
            {
                throw new NotSupportedException();
            }

            // Used to track permanent deletions so we can bulk delete from the index
            // when needed. For example, when emptying the recycle bin, else it will
            // individually update the index which will be much slower.
            HashSet<int> deleteBatch = null;

            foreach (var payload in (MediaCacheRefresher.JsonPayload[])args.MessageObject)
            {
                if (payload.ChangeTypes.HasType(TreeChangeTypes.Remove))
                {
                    if (deleteBatch == null)
                    {
                        deleteBatch = new HashSet<int>();
                    }

                    deleteBatch.Add(payload.Id);
                }
                else if (payload.ChangeTypes.HasType(TreeChangeTypes.RefreshAll))
                {
                    // ExamineEvents does not support RefreshAll
                    // just ignore that payload
                    // so what?!
                }
                else // RefreshNode or RefreshBranch (maybe trashed)
                {
                    if (deleteBatch != null && deleteBatch.Contains(payload.Id))
                    {
                        // the same node has already been deleted, to ensure ordering is
                        // handled, we'll need to execute all queued deleted items now
                        // and reset the deleted items list.
                        _sbnIndexingHandler.DeleteIndexForEntities(deleteBatch, false);
                        deleteBatch = null;
                    }

                    var media = _mediaService.GetById(payload.Id);
                    if (media == null)
                    {
                        // gone fishing, remove entirely
                        _sbnIndexingHandler.DeleteIndexForEntity(payload.Id, false);
                        continue;
                    }

                    if (media.Trashed)
                    {
                        _sbnIndexingHandler.DeleteIndexForEntity(payload.Id, true);
                    }

                    // just that media
                    _sbnIndexingHandler.ReIndexForMedia(media, !media.Trashed);

                    // branch
                    if (payload.ChangeTypes.HasType(TreeChangeTypes.RefreshBranch))
                    {
                        const int pageSize = 500;
                        var page = 0;
                        var total = long.MaxValue;
                        while (page * pageSize < total)
                        {
                            var descendants = _mediaService.GetPagedDescendants(media.Id, page++, pageSize, out total);
                            foreach (var descendant in descendants)
                            {
                                _sbnIndexingHandler.ReIndexForMedia(descendant, !descendant.Trashed);
                            }
                        }
                    }
                }                
            }

            if (deleteBatch != null)
            {
                // process the delete batch
                _sbnIndexingHandler.DeleteIndexForEntities(deleteBatch, false);
            }
        }
    }
}
