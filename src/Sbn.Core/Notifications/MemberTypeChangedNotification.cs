using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Services.Changes;

namespace Sbn.Cms.Core.Notifications
{
    public class MemberTypeChangedNotification : ContentTypeChangeNotification<IMemberType>
    {
        public MemberTypeChangedNotification(ContentTypeChange<IMemberType> target, EventMessages messages) : base(target, messages)
        {
        }

        public MemberTypeChangedNotification(IEnumerable<ContentTypeChange<IMemberType>> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
