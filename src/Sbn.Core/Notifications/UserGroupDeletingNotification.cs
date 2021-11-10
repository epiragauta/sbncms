// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models.Membership;

namespace Sbn.Cms.Core.Notifications
{
    public sealed class UserGroupDeletingNotification : DeletingNotification<IUserGroup>
    {
        public UserGroupDeletingNotification(IUserGroup target, EventMessages messages) : base(target, messages)
        {
        }

        public UserGroupDeletingNotification(IEnumerable<IUserGroup> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
