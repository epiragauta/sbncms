// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Events;

namespace Sbn.Cms.Core.Notifications
{
    public abstract class DeletedNotification<T> : EnumerableObjectNotification<T>
    {
        protected DeletedNotification(T target, EventMessages messages) : base(target, messages)
        {
        }

        protected DeletedNotification(IEnumerable<T> target, EventMessages messages) : base(target, messages)
        {
        }

        public IEnumerable<T> DeletedEntities => Target;
    }
}
