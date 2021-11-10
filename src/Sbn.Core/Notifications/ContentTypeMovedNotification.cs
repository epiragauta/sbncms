using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class ContentTypeMovedNotification : MovedNotification<IContentType>
    {
        public ContentTypeMovedNotification(MoveEventInfo<IContentType> target, EventMessages messages) : base(target, messages)
        {
        }

        public ContentTypeMovedNotification(IEnumerable<MoveEventInfo<IContentType>> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
