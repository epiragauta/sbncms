// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class PartialViewDeletingNotification : DeletingNotification<IPartialView>
    {
        public PartialViewDeletingNotification(IPartialView target, EventMessages messages) : base(target, messages)
        {
        }

        public PartialViewDeletingNotification(IEnumerable<IPartialView> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
