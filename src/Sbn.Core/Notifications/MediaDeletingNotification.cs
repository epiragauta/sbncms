// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public sealed class MediaDeletingNotification : DeletingNotification<IMedia>
    {
        public MediaDeletingNotification(IMedia target, EventMessages messages) : base(target, messages)
        {
        }

        public MediaDeletingNotification(IEnumerable<IMedia> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
