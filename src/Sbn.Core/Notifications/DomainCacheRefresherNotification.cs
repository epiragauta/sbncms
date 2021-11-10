using Sbn.Cms.Core.Sync;

namespace Sbn.Cms.Core.Notifications
{
    public class DomainCacheRefresherNotification : CacheRefresherNotification
    {
        public DomainCacheRefresherNotification(object messageObject, MessageType messageType) : base(messageObject, messageType)
        {
        }
    }
}
