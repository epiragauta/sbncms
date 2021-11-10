using System;
using Sbn.Cms.Core.Cache;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Notifications;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Sync;

namespace Sbn.Cms.Infrastructure.Search
{
    public sealed class MemberIndexingNotificationHandler : INotificationHandler<MemberCacheRefresherNotification>
    {
        private readonly ISbnIndexingHandler _sbnIndexingHandler;
        private readonly IMemberService _memberService;

        public MemberIndexingNotificationHandler(ISbnIndexingHandler sbnIndexingHandler, IMemberService memberService)
        {
            _sbnIndexingHandler = sbnIndexingHandler ?? throw new ArgumentNullException(nameof(sbnIndexingHandler));
            _memberService = memberService ?? throw new ArgumentNullException(nameof(memberService));
        }

        public void Handle(MemberCacheRefresherNotification args)
        {
            if (!_sbnIndexingHandler.Enabled)
            {
                return;
            }

            if (Suspendable.ExamineEvents.CanIndex == false)
            {
                return;
            }

            switch (args.MessageType)
            {
                case MessageType.RefreshById:
                    var c1 = _memberService.GetById((int)args.MessageObject);
                    if (c1 != null)
                    {
                        _sbnIndexingHandler.ReIndexForMember(c1);
                    }
                    break;
                case MessageType.RemoveById:

                    // This is triggered when the item is permanently deleted

                    _sbnIndexingHandler.DeleteIndexForEntity((int)args.MessageObject, false);
                    break;
                case MessageType.RefreshByInstance:
                    if (args.MessageObject is IMember c3)
                    {
                        _sbnIndexingHandler.ReIndexForMember(c3);
                    }
                    break;
                case MessageType.RemoveByInstance:

                    // This is triggered when the item is permanently deleted

                    if (args.MessageObject is IMember c4)
                    {
                        _sbnIndexingHandler.DeleteIndexForEntity(c4.Id, false);
                    }
                    break;
                case MessageType.RefreshByPayload:
                    var payload = (MemberCacheRefresher.JsonPayload[])args.MessageObject;
                    foreach (var p in payload)
                    {
                        if (p.Removed)
                        {
                            _sbnIndexingHandler.DeleteIndexForEntity(p.Id, false);
                        }
                        else
                        {
                            var m = _memberService.GetById(p.Id);
                            if (m != null)
                            {
                                _sbnIndexingHandler.ReIndexForMember(m);
                            }
                        }
                    }
                    break;
                case MessageType.RefreshAll:
                case MessageType.RefreshByJson:
                default:
                    //We don't support these, these message types will not fire for unpublished content
                    break;
            }
        }
    }
}
