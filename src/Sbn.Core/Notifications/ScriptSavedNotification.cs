// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class ScriptSavedNotification : SavedNotification<IScript>
    {
        public ScriptSavedNotification(IScript target, EventMessages messages) : base(target, messages)
        {
        }

        public ScriptSavedNotification(IEnumerable<IScript> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
