// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public sealed class ContentSavingNotification : SavingNotification<IContent>
    {
        public ContentSavingNotification(IContent target, EventMessages messages) : base(target, messages)
        {
        }

        public ContentSavingNotification(IEnumerable<IContent> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
