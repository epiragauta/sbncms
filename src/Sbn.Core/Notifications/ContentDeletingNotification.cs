// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public sealed class ContentDeletingNotification : DeletingNotification<IContent>
    {
        public ContentDeletingNotification(IContent target, EventMessages messages) : base(target, messages)
        {
        }

        public ContentDeletingNotification(IEnumerable<IContent> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
