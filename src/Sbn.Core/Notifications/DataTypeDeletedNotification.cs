using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class DataTypeDeletedNotification : DeletedNotification<IDataType>
    {
        public DataTypeDeletedNotification(IDataType target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
