using Sbn.Cms.Core.Sync;

namespace Sbn.Cms.Core.Notifications
{
    public class UserCacheRefresherNotification : CacheRefresherNotification
    {
        public UserCacheRefresherNotification(object messageObject, MessageType messageType) : base(messageObject, messageType)
        {
        }
    }
}
