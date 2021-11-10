// Copyright (c) Sbn.
// See LICENSE for more details.

using Sbn.Cms.Core.Events;

namespace Sbn.Cms.Core.Notifications
{
    public abstract class RollingBackNotification<T> : CancelableObjectNotification<T> where T : class
    {
        protected RollingBackNotification(T target, EventMessages messages) : base(target, messages)
        {
        }

        public T Entity => Target;
    }
}
