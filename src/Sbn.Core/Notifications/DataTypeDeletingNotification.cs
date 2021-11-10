using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class DataTypeDeletingNotification : DeletingNotification<IDataType>
    {
        public DataTypeDeletingNotification(IDataType target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
