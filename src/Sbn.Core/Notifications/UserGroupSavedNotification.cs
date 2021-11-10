// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models.Membership;

namespace Sbn.Cms.Core.Notifications
{
    public sealed class UserGroupSavedNotification : SavedNotification<IUserGroup>
    {
        public UserGroupSavedNotification(IUserGroup target, EventMessages messages) : base(target, messages)
        {
        }

        public UserGroupSavedNotification(IEnumerable<IUserGroup> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
