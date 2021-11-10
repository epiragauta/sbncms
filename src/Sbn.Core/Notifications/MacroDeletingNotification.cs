using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class MacroDeletingNotification : DeletingNotification<IMacro>
    {
        public MacroDeletingNotification(IMacro target, EventMessages messages) : base(target, messages)
        {
        }

        public MacroDeletingNotification(IEnumerable<IMacro> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
