using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class MediaTypeMovedNotification : MovedNotification<IMediaType>
    {
        public MediaTypeMovedNotification(MoveEventInfo<IMediaType> target, EventMessages messages) : base(target, messages)
        {
        }

        public MediaTypeMovedNotification(IEnumerable<MoveEventInfo<IMediaType>> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
