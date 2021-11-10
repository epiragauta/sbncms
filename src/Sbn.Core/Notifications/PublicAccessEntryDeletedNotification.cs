// Copyright (c) Sbn.
// See LICENSE for more details.

using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public sealed class PublicAccessEntryDeletedNotification : DeletedNotification<PublicAccessEntry>
    {
        public PublicAccessEntryDeletedNotification(PublicAccessEntry target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
