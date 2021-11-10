// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class PartialViewSavingNotification : SavingNotification<IPartialView>
    {
        public PartialViewSavingNotification(IPartialView target, EventMessages messages) : base(target, messages)
        {
        }

        public PartialViewSavingNotification(IEnumerable<IPartialView> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
