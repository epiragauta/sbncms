// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class DictionaryItemDeletingNotification : DeletingNotification<IDictionaryItem>
    {
        public DictionaryItemDeletingNotification(IDictionaryItem target, EventMessages messages) : base(target, messages)
        {
        }

        public DictionaryItemDeletingNotification(IEnumerable<IDictionaryItem> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
