// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Events;

namespace Sbn.Cms.Core.Notifications
{
    public sealed class UserGroupWithUsersSavingNotification : SavingNotification<UserGroupWithUsers>
    {
        public UserGroupWithUsersSavingNotification(UserGroupWithUsers target, EventMessages messages) : base(target, messages)
        {
        }

        public UserGroupWithUsersSavingNotification(IEnumerable<UserGroupWithUsers> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
