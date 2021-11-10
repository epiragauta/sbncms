using Sbn.Cms.Core.Sync;

namespace Sbn.Cms.Core.Notifications
{
    public class MediaCacheRefresherNotification : CacheRefresherNotification
    {
        public MediaCacheRefresherNotification(object messageObject, MessageType messageType) : base(messageObject, messageType)
        {
        }
    }
}
