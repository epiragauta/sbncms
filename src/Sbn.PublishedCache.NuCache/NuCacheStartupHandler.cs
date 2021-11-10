using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Notifications;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Infrastructure.PublishedCache.Persistence;

namespace Sbn.Cms.Infrastructure.PublishedCache
{
    /// <summary>
    /// Rebuilds the database cache if required when the serializer changes
    /// </summary>
    public class NuCacheStartupHandler : INotificationHandler<SbnApplicationStartingNotification>
    {
        private readonly INuCacheContentService _nuCacheContentService;
        private readonly IRuntimeState _runtimeState;

        public NuCacheStartupHandler(
            INuCacheContentService nuCacheContentService,
            IRuntimeState runtimeState)
        {
            _nuCacheContentService = nuCacheContentService;
            _runtimeState = runtimeState;
        }

        public void Handle(SbnApplicationStartingNotification notification)
        {
            if (_runtimeState.Level == Core.RuntimeLevel.Run)
            {
                _nuCacheContentService.RebuildDatabaseCacheIfSerializerChanged();
            }
        }

        
    }
}
