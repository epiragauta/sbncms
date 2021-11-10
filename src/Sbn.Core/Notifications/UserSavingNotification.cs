// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models.Membership;

namespace Sbn.Cms.Core.Notifications
{
    public sealed class UserSavingNotification : SavingNotification<IUser>
    {
        public UserSavingNotification(IUser target, EventMessages messages) : base(target, messages)
        {
        }

        public UserSavingNotification(IEnumerable<IUser> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
