using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class MemberGroupDeletedNotification : DeletedNotification<IMemberGroup>
    {
        public MemberGroupDeletedNotification(IMemberGroup target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
