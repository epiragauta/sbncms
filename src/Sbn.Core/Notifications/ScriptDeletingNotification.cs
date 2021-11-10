// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class ScriptDeletingNotification : DeletingNotification<IScript>
    {
        public ScriptDeletingNotification(IScript target, EventMessages messages) : base(target, messages)
        {
        }

        public ScriptDeletingNotification(IEnumerable<IScript> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
