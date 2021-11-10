using Sbn.Cms.Core.Sync;

namespace Sbn.Cms.Core.Notifications
{
    public class LanguageCacheRefresherNotification : CacheRefresherNotification
    {
        public LanguageCacheRefresherNotification(object messageObject, MessageType messageType) : base(messageObject, messageType)
        {
        }
    }
}
