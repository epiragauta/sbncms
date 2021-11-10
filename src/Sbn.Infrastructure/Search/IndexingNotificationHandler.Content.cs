using System;
using System.Collections.Generic;
using Sbn.Cms.Core;
using Sbn.Cms.Core.Cache;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Notifications;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Services.Changes;
using Sbn.Cms.Core.Sync;
using Sbn.Extensions;

namespace Sbn.Cms.Infrastructure.Search
{
    public sealed class ContentIndexingNotificationHandler : INotificationHandler<ContentCacheRefresherNotification>
    {
        private readonly ISbnIndexingHandler _sbnIndexingHandler;
        private readonly IContentService _contentService;

        public ContentIndexingNotificationHandler(ISbnIndexingHandler sbnIndexingHandler, IContentService contentService)
        {
            _sbnIndexingHandler = sbnIndexingHandler ?? throw new ArgumentNullException(nameof(sbnIndexingHandler));
            _contentService = contentService ?? throw new ArgumentNullException(nameof(contentService));
        }

        /// <summary>
        /// Updates indexes based on content changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void Handle(ContentCacheRefresherNotification args)
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

            foreach (var payload in (ContentCacheRefresher.JsonPayload[])args.MessageObject)
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

                    // TODO: Rebuild the index at this point?
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

                    // don't try to be too clever - refresh entirely
                    // there has to be race conditions in there ;-(

                    var content = _contentService.GetById(payload.Id);
                    if (content == null)
                    {
                        // gone fishing, remove entirely from all indexes (with descendants)
                        _sbnIndexingHandler.DeleteIndexForEntity(payload.Id, false);
                        continue;
                    }

                    IContent published = null;
                    if (content.Published && _contentService.IsPathPublished(content))
                    {
                        published = content;
                    }

                    if (published == null)
                    {
                        _sbnIndexingHandler.DeleteIndexForEntity(payload.Id, true);
                    }

                    // just that content
                    _sbnIndexingHandler.ReIndexForContent(content, published != null);

                    // branch
                    if (payload.ChangeTypes.HasType(TreeChangeTypes.RefreshBranch))
                    {
                        var masked = published == null ? null : new List<int>();
                        const int pageSize = 500;
                        var page = 0;
                        var total = long.MaxValue;
                        while (page * pageSize < total)
                        {
                            var descendants = _contentService.GetPagedDescendants(content.Id, page++, pageSize, out total,
                                //order by shallowest to deepest, this allows us to check it's published state without checking every item
                                ordering: Ordering.By("Path", Direction.Ascending));

                            foreach (var descendant in descendants)
                            {
                                published = null;
                                if (masked != null) // else everything is masked
                                {
                                    if (masked.Contains(descendant.ParentId) || !descendant.Published)
                                    {
                                        masked.Add(descendant.Id);
                                    }
                                    else
                                    {
                                        published = descendant;
                                    }
                                }

                                _sbnIndexingHandler.ReIndexForContent(descendant, published != null);
                            }
                        }
                    }
                }

                // NOTE
                //
                // DeleteIndexForEntity is handled by SbnContentIndexer.DeleteFromIndex() which takes
                //  care of also deleting the descendants
                //
                // ReIndexForContent is NOT taking care of descendants so we have to reload everything
                //  again in order to process the branch - we COULD improve that by just reloading the
                //  XML from database instead of reloading content & re-serializing!
                //
                // BUT ... pretty sure it is! see test "Index_Delete_Index_Item_Ensure_Heirarchy_Removed"
            }

            if (deleteBatch != null)
            {
                // process the delete batch
                _sbnIndexingHandler.DeleteIndexForEntities(deleteBatch, false);
            }
        }
    }
}
