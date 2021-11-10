using Sbn.Cms.Core.Sync;

namespace Sbn.Cms.Core.Notifications
{
    public class RelationTypeCacheRefresherNotification : CacheRefresherNotification
    {
        public RelationTypeCacheRefresherNotification(object messageObject, MessageType messageType) : base(messageObject, messageType)
        {
        }
    }
}
