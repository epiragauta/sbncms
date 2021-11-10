// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Events;

namespace Sbn.Cms.Core.Notifications
{
    public abstract class EmptyingRecycleBinNotification<T> : StatefulNotification, ICancelableNotification where T : class
    {
        protected EmptyingRecycleBinNotification(IEnumerable<T> deletedEntities, EventMessages messages)
        {
            DeletedEntities = deletedEntities;
            Messages = messages;
        }

        public IEnumerable<T> DeletedEntities { get; }

        public EventMessages Messages { get; }

        public bool Cancel { get; set; }
    }
}
