// Copyright (c) Sbn.
// See LICENSE for more details.

using Sbn.Cms.Core.Events;

namespace Sbn.Cms.Core.Notifications
{
    public abstract class CreatedNotification<T> : ObjectNotification<T> where T : class
    {
        protected CreatedNotification(T target, EventMessages messages) : base(target, messages)
        {
        }

        public T CreatedEntity => Target;
    }
}
