// Copyright (c) Sbn.
// See LICENSE for more details

using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class StylesheetSavingNotification : SavingNotification<IStylesheet>
    {
        public StylesheetSavingNotification(IStylesheet target, EventMessages messages) : base(target, messages)
        {
        }

        public StylesheetSavingNotification(IEnumerable<IStylesheet> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
