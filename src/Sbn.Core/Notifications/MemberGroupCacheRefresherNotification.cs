using Sbn.Cms.Core.Sync;

namespace Sbn.Cms.Core.Notifications
{
    public class MemberGroupCacheRefresherNotification : CacheRefresherNotification
    {
        public MemberGroupCacheRefresherNotification(object messageObject, MessageType messageType) : base(messageObject, messageType)
        {
        }
    }
}
