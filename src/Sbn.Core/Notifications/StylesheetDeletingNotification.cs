// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class StylesheetDeletingNotification : DeletingNotification<IStylesheet>
    {
        public StylesheetDeletingNotification(IStylesheet target, EventMessages messages) : base(target, messages)
        {
        }

        public StylesheetDeletingNotification(IEnumerable<IStylesheet> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
