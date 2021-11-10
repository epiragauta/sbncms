// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public sealed class MediaMovingToRecycleBinNotification : MovingToRecycleBinNotification<IMedia>
    {
        public MediaMovingToRecycleBinNotification(MoveEventInfo<IMedia> target, EventMessages messages) : base(target, messages)
        {
        }

        public MediaMovingToRecycleBinNotification(IEnumerable<MoveEventInfo<IMedia>> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
