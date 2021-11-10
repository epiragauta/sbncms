// Copyright (c) Sbn.
// See LICENSE for more details.

using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public sealed class MemberDeletedNotification : DeletedNotification<IMember>
    {
        public MemberDeletedNotification(IMember target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
