// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Events;

namespace Sbn.Cms.Core.Notifications
{
    public sealed class UserGroupWithUsersSavedNotification : SavedNotification<UserGroupWithUsers>
    {
        public UserGroupWithUsersSavedNotification(UserGroupWithUsers target, EventMessages messages) : base(target, messages)
        {
        }

        public UserGroupWithUsersSavedNotification(IEnumerable<UserGroupWithUsers> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
