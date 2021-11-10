// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class DictionaryItemSavedNotification : SavedNotification<IDictionaryItem>
    {
        public DictionaryItemSavedNotification(IDictionaryItem target, EventMessages messages) : base(target, messages)
        {
        }

        public DictionaryItemSavedNotification(IEnumerable<IDictionaryItem> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
