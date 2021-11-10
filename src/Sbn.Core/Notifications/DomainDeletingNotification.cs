// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class DomainDeletingNotification : DeletingNotification<IDomain>
    {
        public DomainDeletingNotification(IDomain target, EventMessages messages) : base(target, messages)
        {
        }

        public DomainDeletingNotification(IEnumerable<IDomain> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
