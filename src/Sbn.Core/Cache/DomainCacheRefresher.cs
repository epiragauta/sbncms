using System;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Notifications;
using Sbn.Cms.Core.PublishedCache;
using Sbn.Cms.Core.Serialization;
using Sbn.Cms.Core.Services.Changes;

namespace Sbn.Cms.Core.Cache
{
    public sealed class DomainCacheRefresher : PayloadCacheRefresherBase<DomainCacheRefresherNotification, DomainCacheRefresher.JsonPayload>
    {
        private readonly IPublishedSnapshotService _publishedSnapshotService;

        public DomainCacheRefresher(
            AppCaches appCaches,
            IJsonSerializer serializer,
            IPublishedSnapshotService publishedSnapshotService,
            IEventAggregator eventAggregator,
            ICacheRefresherNotificationFactory factory)
            : base(appCaches, serializer, eventAggregator, factory)
        {
            _publishedSnapshotService = publishedSnapshotService;
        }

        #region Define

        public static readonly Guid UniqueId = Guid.Parse("11290A79-4B57-4C99-AD72-7748A3CF38AF");

        public override Guid RefresherUniqueId => UniqueId;

        public override string Name => "Domain Cache Refresher";

        #endregion

        #region Refresher

        public override void Refresh(JsonPayload[] payloads)
        {
            ClearAllIsolatedCacheByEntityType<IDomain>();

            // note: must do what's above FIRST else the repositories still have the old cached
            // content and when the PublishedCachesService is notified of changes it does not see
            // the new content...

            // notify
            _publishedSnapshotService.Notify(payloads);
            // then trigger event
            base.Refresh(payloads);
        }

        // these events should never trigger
        // everything should be PAYLOAD/JSON

        public override void RefreshAll() => throw new NotSupportedException();

        public override void Refresh(int id) => throw new NotSupportedException();

        public override void Refresh(Guid id) => throw new NotSupportedException();

        public override void Remove(int id) => throw new NotSupportedException();

        #endregion

        #region Json

        public class JsonPayload
        {
            public JsonPayload(int id, DomainChangeTypes changeType)
            {
                Id = id;
                ChangeType = changeType;
            }

            public int Id { get; }

            public DomainChangeTypes ChangeType { get; }
        }

        #endregion

    }
}
