// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class DomainSavedNotification : SavedNotification<IDomain>
    {
        public DomainSavedNotification(IDomain target, EventMessages messages) : base(target, messages)
        {
        }

        public DomainSavedNotification(IEnumerable<IDomain> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
