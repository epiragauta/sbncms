using Sbn.Cms.Core.Sync;

namespace Sbn.Cms.Core.Notifications
{
    public class DictionaryCacheRefresherNotification : CacheRefresherNotification
    {
        public DictionaryCacheRefresherNotification(object messageObject, MessageType messageType) : base(messageObject, messageType)
        {
        }
    }
}
