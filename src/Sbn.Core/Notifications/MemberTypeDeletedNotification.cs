using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class MemberTypeDeletedNotification : DeletedNotification<IMemberType>
    {
        public MemberTypeDeletedNotification(IMemberType target, EventMessages messages) : base(target, messages)
        {
        }

        public MemberTypeDeletedNotification(IEnumerable<IMemberType> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
