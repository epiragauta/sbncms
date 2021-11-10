using Sbn.Cms.Core.Notifications;
using Sbn.Cms.Core.Sync;

namespace Sbn.Cms.Core.Cache
{
    /// <summary>
    /// Factory for creating cache refresher notification instances
    /// </summary>
    public interface ICacheRefresherNotificationFactory
    {
        /// <summary>
        /// Creates a <see cref="ICacheRefresherNotification"/>
        /// </summary>
        /// <typeparam name="TNotification">The <see cref="ICacheRefresherNotification"/> to create</typeparam>
        TNotification Create<TNotification>(object msgObject, MessageType type) where TNotification : CacheRefresherNotification;
    }
}
