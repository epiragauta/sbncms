// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public sealed class ContentMovedToRecycleBinNotification : MovedToRecycleBinNotification<IContent>
    {
        public ContentMovedToRecycleBinNotification(MoveEventInfo<IContent> target, EventMessages messages) : base(target, messages)
        {
        }

        public ContentMovedToRecycleBinNotification(IEnumerable<MoveEventInfo<IContent>> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
