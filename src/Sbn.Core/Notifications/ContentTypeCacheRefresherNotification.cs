using Sbn.Cms.Core.Sync;

namespace Sbn.Cms.Core.Notifications
{
    public class ContentTypeCacheRefresherNotification : CacheRefresherNotification
    {
        public ContentTypeCacheRefresherNotification(object messageObject, MessageType messageType) : base(messageObject, messageType)
        {
        }
    }
}
