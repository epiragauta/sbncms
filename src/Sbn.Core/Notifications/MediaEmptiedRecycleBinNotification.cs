// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public sealed class MediaEmptiedRecycleBinNotification : EmptiedRecycleBinNotification<IMedia>
    {
        public MediaEmptiedRecycleBinNotification(IEnumerable<IMedia> deletedEntities,EventMessages messages) : base(deletedEntities, messages)
        {
        }
    }
}
