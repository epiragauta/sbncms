using Sbn.Cms.Core.Sync;

namespace Sbn.Cms.Core.Notifications
{
    public class MemberCacheRefresherNotification : CacheRefresherNotification
    {
        public MemberCacheRefresherNotification(object messageObject, MessageType messageType) : base(messageObject, messageType)
        {
        }
    }
}
