// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Events;

namespace Sbn.Cms.Core.Notifications
{
    public abstract class EmptiedRecycleBinNotification<T> : StatefulNotification where T : class
    {
        protected EmptiedRecycleBinNotification(IEnumerable<T> deletedEntities, EventMessages messages)
        {
            DeletedEntities = deletedEntities;
            Messages = messages;
        }

        public IEnumerable<T> DeletedEntities { get; }

        public EventMessages Messages { get; }
    }
}
