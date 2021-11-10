// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public sealed class MemberSavedNotification : SavedNotification<IMember>
    {
        public MemberSavedNotification(IMember target, EventMessages messages) : base(target, messages)
        {
        }

        public MemberSavedNotification(IEnumerable<IMember> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
