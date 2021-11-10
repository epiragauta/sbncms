// Copyright (c) Sbn.
// See LICENSE for more details.

using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class DomainDeletedNotification : DeletedNotification<IDomain>
    {
        public DomainDeletedNotification(IDomain target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
