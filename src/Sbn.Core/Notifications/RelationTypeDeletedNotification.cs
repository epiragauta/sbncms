// Copyright (c) Sbn.
// See LICENSE for more details.

using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class RelationTypeDeletedNotification : DeletedNotification<IRelationType>
    {
        public RelationTypeDeletedNotification(IRelationType target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
