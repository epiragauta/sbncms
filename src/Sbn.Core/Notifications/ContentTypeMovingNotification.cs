using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class ContentTypeMovingNotification : MovingNotification<IContentType>
    {
        public ContentTypeMovingNotification(MoveEventInfo<IContentType> target, EventMessages messages) : base(target, messages)
        {
        }

        public ContentTypeMovingNotification(IEnumerable<MoveEventInfo<IContentType>> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
