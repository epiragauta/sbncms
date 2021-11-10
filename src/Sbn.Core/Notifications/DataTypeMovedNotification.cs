using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class DataTypeMovedNotification : MovedNotification<IDataType>
    {
        public DataTypeMovedNotification(MoveEventInfo<IDataType> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
