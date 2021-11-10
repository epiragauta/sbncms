// Copyright (c) Sbn.
// See LICENSE for more details

using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class StylesheetSavedNotification : SavedNotification<IStylesheet>
    {
        public StylesheetSavedNotification(IStylesheet target, EventMessages messages) : base(target, messages)
        {
        }

        public StylesheetSavedNotification(IEnumerable<IStylesheet> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
