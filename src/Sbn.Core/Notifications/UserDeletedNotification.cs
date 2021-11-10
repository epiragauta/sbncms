// Copyright (c) Sbn.
// See LICENSE for more details.

using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models.Membership;

namespace Sbn.Cms.Core.Notifications
{
    public sealed class UserDeletedNotification : DeletedNotification<IUser>
    {
        public UserDeletedNotification(IUser target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
