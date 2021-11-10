using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class MemberTypeSavedNotification : SavedNotification<IMemberType>
    {
        public MemberTypeSavedNotification(IMemberType target, EventMessages messages) : base(target, messages)
        {
        }

        public MemberTypeSavedNotification(IEnumerable<IMemberType> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
