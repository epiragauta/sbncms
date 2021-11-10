// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class RelationSavingNotification : SavingNotification<IRelation>
    {
        public RelationSavingNotification(IRelation target, EventMessages messages) : base(target, messages)
        {
        }

        public RelationSavingNotification(IEnumerable<IRelation> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
