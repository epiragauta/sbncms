// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class TemplateDeletingNotification : DeletingNotification<ITemplate>
    {
        public TemplateDeletingNotification(ITemplate target, EventMessages messages) : base(target, messages)
        {
        }

        public TemplateDeletingNotification(IEnumerable<ITemplate> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
