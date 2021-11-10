using System;
using System.Collections.Generic;
using System.ComponentModel;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Services.Changes;

namespace Sbn.Cms.Core.Notifications
{
    [Obsolete("This is only used for the internal cache and will change, use tree change notifications instead")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class MemberTypeRefreshedNotification : ContentTypeRefreshNotification<IMemberType>
    {
        public MemberTypeRefreshedNotification(ContentTypeChange<IMemberType> target, EventMessages messages) : base(target, messages)
        {
        }

        public MemberTypeRefreshedNotification(IEnumerable<ContentTypeChange<IMemberType>> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
