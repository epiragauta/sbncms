using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class DataTypeSavingNotification : SavingNotification<IDataType>
    {
        public DataTypeSavingNotification(IDataType target, EventMessages messages) : base(target, messages)
        {
        }

        public DataTypeSavingNotification(IEnumerable<IDataType> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
