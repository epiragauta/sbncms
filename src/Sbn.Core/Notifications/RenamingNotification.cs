// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Events;

namespace Sbn.Cms.Core.Notifications
{
    public abstract class RenamingNotification<T> : CancelableEnumerableObjectNotification<T>
    {
        protected RenamingNotification(T target, EventMessages messages) : base(target, messages)
        {
        }

        protected RenamingNotification(IEnumerable<T> target, EventMessages messages) : base(target, messages)
        {
        }

        public IEnumerable<T> Entities => Target;
    }
}
