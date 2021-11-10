using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class MemberGroupDeletingNotification : DeletingNotification<IMemberGroup>
    {
        public MemberGroupDeletingNotification(IMemberGroup target, EventMessages messages) : base(target, messages)
        {
        }

        public MemberGroupDeletingNotification(IEnumerable<IMemberGroup> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
