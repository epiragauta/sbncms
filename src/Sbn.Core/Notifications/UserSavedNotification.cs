// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models.Membership;

namespace Sbn.Cms.Core.Notifications
{
    public sealed class UserSavedNotification : SavedNotification<IUser>
    {
        public UserSavedNotification(IUser target, EventMessages messages) : base(target, messages)
        {
        }

        public UserSavedNotification(IEnumerable<IUser> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
