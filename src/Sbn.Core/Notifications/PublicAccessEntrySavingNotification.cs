// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public sealed class PublicAccessEntrySavingNotification : SavingNotification<PublicAccessEntry>
    {
        public PublicAccessEntrySavingNotification(PublicAccessEntry target, EventMessages messages) : base(target, messages)
        {
        }

        public PublicAccessEntrySavingNotification(IEnumerable<PublicAccessEntry> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
