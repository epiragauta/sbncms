using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class MemberTypeMovedNotification : MovedNotification<IMemberType>
    {
        public MemberTypeMovedNotification(MoveEventInfo<IMemberType> target, EventMessages messages) : base(target, messages)
        {
        }

        public MemberTypeMovedNotification(IEnumerable<MoveEventInfo<IMemberType>> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
