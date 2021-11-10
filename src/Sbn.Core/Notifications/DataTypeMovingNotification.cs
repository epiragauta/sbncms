using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class DataTypeMovingNotification : MovingNotification<IDataType>
    {
        public DataTypeMovingNotification(MoveEventInfo<IDataType> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
