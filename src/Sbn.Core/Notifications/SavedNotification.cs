// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Events;

namespace Sbn.Cms.Core.Notifications
{
    public abstract class SavedNotification<T> : EnumerableObjectNotification<T>
    {
        protected SavedNotification(T target, EventMessages messages) : base(target, messages)
        {
        }

        protected SavedNotification(IEnumerable<T> target, EventMessages messages) : base(target, messages)
        {
        }

        public IEnumerable<T> SavedEntities => Target;
    }
}
