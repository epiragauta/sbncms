using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class MacroSavedNotification : SavedNotification<IMacro>
    {
        public MacroSavedNotification(IMacro target, EventMessages messages) : base(target, messages)
        {
        }

        public MacroSavedNotification(IEnumerable<IMacro> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
