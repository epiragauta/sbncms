// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public sealed class PublicAccessEntrySavedNotification : SavedNotification<PublicAccessEntry>
    {
        public PublicAccessEntrySavedNotification(PublicAccessEntry target, EventMessages messages) : base(target, messages)
        {
        }

        public PublicAccessEntrySavedNotification(IEnumerable<PublicAccessEntry> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
