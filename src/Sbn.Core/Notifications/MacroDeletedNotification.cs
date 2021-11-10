using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class MacroDeletedNotification : DeletedNotification<IMacro>
    {
        public MacroDeletedNotification(IMacro target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
