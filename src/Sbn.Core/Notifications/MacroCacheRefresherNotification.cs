using Sbn.Cms.Core.Sync;

namespace Sbn.Cms.Core.Notifications
{
    public class MacroCacheRefresherNotification : CacheRefresherNotification
    {
        public MacroCacheRefresherNotification(object messageObject, MessageType messageType) : base(messageObject, messageType)
        {
        }
    }
}
