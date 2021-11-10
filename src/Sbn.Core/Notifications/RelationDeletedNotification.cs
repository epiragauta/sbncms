// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class RelationDeletedNotification : DeletedNotification<IRelation>
    {
        public RelationDeletedNotification(IRelation target, EventMessages messages) : base(target, messages)
        {
        }

        public RelationDeletedNotification(IEnumerable<IRelation> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
