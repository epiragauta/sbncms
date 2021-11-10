// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public sealed class MemberSavingNotification : SavingNotification<IMember>
    {
        public MemberSavingNotification(IMember target, EventMessages messages) : base(target, messages)
        {
        }

        public MemberSavingNotification(IEnumerable<IMember> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
