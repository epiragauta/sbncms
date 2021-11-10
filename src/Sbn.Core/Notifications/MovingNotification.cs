// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Events;

namespace Sbn.Cms.Core.Notifications
{
    public abstract class MovingNotification<T> : CancelableObjectNotification<IEnumerable<MoveEventInfo<T>>>
    {
        protected MovingNotification(MoveEventInfo<T> target, EventMessages messages) : base(new[] {target}, messages)
        {
        }

        protected MovingNotification(IEnumerable<MoveEventInfo<T>> target, EventMessages messages) : base(target, messages)
        {
        }

        public IEnumerable<MoveEventInfo<T>> MoveInfoCollection => Target;
    }
}
