// Copyright (c) Sbn.
// See LICENSE for more details.

using Sbn.Cms.Core.Events;

namespace Sbn.Cms.Core.Notifications
{
    public abstract class ObjectNotification<T> : StatefulNotification where T : class
    {
        protected ObjectNotification(T target, EventMessages messages)
        {
            Messages = messages;
            Target = target;
        }

        public EventMessages Messages { get; }

        protected T Target { get; }
    }
}
