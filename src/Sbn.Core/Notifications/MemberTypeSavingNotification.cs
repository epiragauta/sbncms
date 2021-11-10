using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class MemberTypeSavingNotification : SavingNotification<IMemberType>
    {
        public MemberTypeSavingNotification(IMemberType target, EventMessages messages) : base(target, messages)
        {
        }

        public MemberTypeSavingNotification(IEnumerable<IMemberType> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
