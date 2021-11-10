using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class MemberGroupSavedNotification : SavedNotification<IMemberGroup>
    {
        public MemberGroupSavedNotification(IMemberGroup target, EventMessages messages) : base(target, messages)
        {
        }

        public MemberGroupSavedNotification(IEnumerable<IMemberGroup> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
