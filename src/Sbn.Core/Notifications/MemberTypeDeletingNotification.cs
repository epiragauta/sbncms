using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class MemberTypeDeletingNotification : DeletingNotification<IMemberType>
    {
        public MemberTypeDeletingNotification(IMemberType target, EventMessages messages) : base(target, messages)
        {
        }

        public MemberTypeDeletingNotification(IEnumerable<IMemberType> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
