// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class LanguageDeletingNotification : DeletingNotification<ILanguage>
    {
        public LanguageDeletingNotification(ILanguage target, EventMessages messages) : base(target, messages)
        {
        }

        public LanguageDeletingNotification(IEnumerable<ILanguage> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
