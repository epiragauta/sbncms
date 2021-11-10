using Sbn.Cms.Core.Sync;

namespace Sbn.Cms.Core.Notifications
{
    public class ContentCacheRefresherNotification : CacheRefresherNotification
    {
        public ContentCacheRefresherNotification(object messageObject, MessageType messageType) : base(messageObject, messageType)
        {
        }
    }
}
