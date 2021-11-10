// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class RelationTypeSavingNotification : SavingNotification<IRelationType>
    {
        public RelationTypeSavingNotification(IRelationType target, EventMessages messages) : base(target, messages)
        {
        }

        public RelationTypeSavingNotification(IEnumerable<IRelationType> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
