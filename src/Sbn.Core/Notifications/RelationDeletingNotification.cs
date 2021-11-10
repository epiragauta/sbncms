// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class RelationDeletingNotification : DeletingNotification<IRelation>
    {
        public RelationDeletingNotification(IRelation target, EventMessages messages) : base(target, messages)
        {
        }

        public RelationDeletingNotification(IEnumerable<IRelation> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
