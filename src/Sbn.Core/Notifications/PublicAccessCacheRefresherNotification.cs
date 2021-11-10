using Sbn.Cms.Core.Sync;

namespace Sbn.Cms.Core.Notifications
{
    public class PublicAccessCacheRefresherNotification : CacheRefresherNotification
    {
        public PublicAccessCacheRefresherNotification(object messageObject, MessageType messageType) : base(messageObject, messageType)
        {
        }
    }
}
