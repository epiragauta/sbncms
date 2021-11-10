using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class MacroSavingNotification : SavingNotification<IMacro>
    {
        public MacroSavingNotification(IMacro target, EventMessages messages) : base(target, messages)
        {
        }

        public MacroSavingNotification(IEnumerable<IMacro> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
