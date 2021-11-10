using Sbn.Cms.Core.Sync;

namespace Sbn.Cms.Core.Notifications
{
    public class TemplateCacheRefresherNotification : CacheRefresherNotification
    {
        public TemplateCacheRefresherNotification(object messageObject, MessageType messageType) : base(messageObject, messageType)
        {
        }
    }
}
