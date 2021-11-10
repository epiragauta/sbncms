// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class RelationTypeSavedNotification : SavedNotification<IRelationType>
    {
        public RelationTypeSavedNotification(IRelationType target, EventMessages messages) : base(target, messages)
        {
        }

        public RelationTypeSavedNotification(IEnumerable<IRelationType> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
