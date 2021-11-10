// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class DictionaryItemSavingNotification : SavingNotification<IDictionaryItem>
    {
        public DictionaryItemSavingNotification(IDictionaryItem target, EventMessages messages) : base(target, messages)
        {
        }

        public DictionaryItemSavingNotification(IEnumerable<IDictionaryItem> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
