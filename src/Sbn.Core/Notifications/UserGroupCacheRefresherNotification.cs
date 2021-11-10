using Sbn.Cms.Core.Sync;

namespace Sbn.Cms.Core.Notifications
{
    public class UserGroupCacheRefresherNotification : CacheRefresherNotification
    {
        public UserGroupCacheRefresherNotification(object messageObject, MessageType messageType) : base(messageObject, messageType)
        {
        }
    }
}
