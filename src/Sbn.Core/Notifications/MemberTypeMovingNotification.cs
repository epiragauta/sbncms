using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class MemberTypeMovingNotification : MovingNotification<IMemberType>
    {
        public MemberTypeMovingNotification(MoveEventInfo<IMemberType> target, EventMessages messages) : base(target, messages)
        {
        }

        public MemberTypeMovingNotification(IEnumerable<MoveEventInfo<IMemberType>> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
