// Copyright (c) Sbn.
// See LICENSE for more details.

using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class ScriptDeletedNotification : DeletedNotification<IScript>
    {
        public ScriptDeletedNotification(IScript target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
