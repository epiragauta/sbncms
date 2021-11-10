// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public sealed class ContentSortingNotification : SortingNotification<IContent>
    {
        public ContentSortingNotification(IEnumerable<IContent> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
