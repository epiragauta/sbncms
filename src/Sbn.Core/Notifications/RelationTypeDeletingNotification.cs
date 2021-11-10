// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class RelationTypeDeletingNotification : DeletingNotification<IRelationType>
    {
        public RelationTypeDeletingNotification(IRelationType target, EventMessages messages) : base(target, messages)
        {
        }

        public RelationTypeDeletingNotification(IEnumerable<IRelationType> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
