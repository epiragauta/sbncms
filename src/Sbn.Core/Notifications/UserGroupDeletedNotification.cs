// Copyright (c) Sbn.
// See LICENSE for more details.

using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models.Membership;

namespace Sbn.Cms.Core.Notifications
{
    public sealed class UserGroupDeletedNotification : DeletedNotification<IUserGroup>
    {
        public UserGroupDeletedNotification(IUserGroup target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
