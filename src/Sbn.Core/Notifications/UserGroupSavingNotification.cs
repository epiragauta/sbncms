// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models.Membership;

namespace Sbn.Cms.Core.Notifications
{
    public sealed class UserGroupSavingNotification : SavingNotification<IUserGroup>
    {
        public UserGroupSavingNotification(IUserGroup target, EventMessages messages) : base(target, messages)
        {
        }

        public UserGroupSavingNotification(IEnumerable<IUserGroup> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
