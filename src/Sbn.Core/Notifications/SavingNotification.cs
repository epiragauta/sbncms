// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Events;

namespace Sbn.Cms.Core.Notifications
{
    public abstract class SavingNotification<T> : CancelableEnumerableObjectNotification<T>
    {
        protected SavingNotification(T target, EventMessages messages) : base(target, messages)
        {
        }

        protected SavingNotification(IEnumerable<T> target, EventMessages messages) : base(target, messages)
        {
        }

        public IEnumerable<T> SavedEntities => Target;
    }
}
