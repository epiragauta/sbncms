using System;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Notifications;

namespace Sbn.Cms.Core.Cache
{
    public sealed class PublicAccessCacheRefresher : CacheRefresherBase<PublicAccessCacheRefresherNotification>
    {
        public PublicAccessCacheRefresher(AppCaches appCaches, IEventAggregator eventAggregator, ICacheRefresherNotificationFactory factory)
            : base(appCaches, eventAggregator, factory)
        { }

        #region Define

        public static readonly Guid UniqueId = Guid.Parse("1DB08769-B104-4F8B-850E-169CAC1DF2EC");

        public override Guid RefresherUniqueId => UniqueId;

        public override string Name => "Public Access Cache Refresher";

        #endregion

        #region Refresher

        public override void Refresh(Guid id)
        {
            ClearAllIsolatedCacheByEntityType<PublicAccessEntry>();
            base.Refresh(id);
        }

        public override void Refresh(int id)
        {
            ClearAllIsolatedCacheByEntityType<PublicAccessEntry>();
            base.Refresh(id);
        }

        public override void RefreshAll()
        {
            ClearAllIsolatedCacheByEntityType<PublicAccessEntry>();
            base.RefreshAll();
        }

        public override void Remove(int id)
        {
            ClearAllIsolatedCacheByEntityType<PublicAccessEntry>();
            base.Remove(id);
        }

        #endregion
    }
}
