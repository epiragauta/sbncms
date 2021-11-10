// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public sealed class ContentEmptiedRecycleBinNotification : EmptiedRecycleBinNotification<IContent>
    {
        public ContentEmptiedRecycleBinNotification(IEnumerable<IContent> deletedEntities, EventMessages messages) : base(deletedEntities, messages)
        {
        }
    }
}
