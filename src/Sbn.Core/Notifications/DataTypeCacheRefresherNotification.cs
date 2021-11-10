using Sbn.Cms.Core.Sync;

namespace Sbn.Cms.Core.Notifications
{
    public class DataTypeCacheRefresherNotification : CacheRefresherNotification
    {
        public DataTypeCacheRefresherNotification(object messageObject, MessageType messageType) : base(messageObject, messageType)
        {
        }
    }
}
