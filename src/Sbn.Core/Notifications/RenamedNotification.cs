// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Events;

namespace Sbn.Cms.Core.Notifications
{
    public abstract class RenamedNotification<T> : EnumerableObjectNotification<T>
    {
        protected RenamedNotification(T target, EventMessages messages) : base(target, messages)
        {
        }

        protected RenamedNotification(IEnumerable<T> target, EventMessages messages) : base(target, messages)
        {
        }

        public IEnumerable<T> Entities => Target;
    }
}
