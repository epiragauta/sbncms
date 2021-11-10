using Sbn.Cms.Core.Sync;

namespace Sbn.Cms.Core.Notifications
{
    public class ApplicationCacheRefresherNotification : CacheRefresherNotification
    {
        public ApplicationCacheRefresherNotification(object messageObject, MessageType messageType) : base(messageObject, messageType)
        {
        }
    }
}
