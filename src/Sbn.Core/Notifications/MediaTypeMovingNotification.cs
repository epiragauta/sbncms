using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class MediaTypeMovingNotification : MovingNotification<IMediaType>
    {
        public MediaTypeMovingNotification(MoveEventInfo<IMediaType> target, EventMessages messages) : base(target, messages)
        {
        }

        public MediaTypeMovingNotification(IEnumerable<MoveEventInfo<IMediaType>> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
