using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class MemberGroupSavingNotification : SavingNotification<IMemberGroup>
    {
        public MemberGroupSavingNotification(IMemberGroup target, EventMessages messages) : base(target, messages)
        {
        }

        public MemberGroupSavingNotification(IEnumerable<IMemberGroup> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
