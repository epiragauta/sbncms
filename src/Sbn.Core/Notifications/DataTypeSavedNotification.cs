using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class DataTypeSavedNotification : SavedNotification<IDataType>
    {
        public DataTypeSavedNotification(IDataType target, EventMessages messages) : base(target, messages)
        {
        }

        public DataTypeSavedNotification(IEnumerable<IDataType> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
