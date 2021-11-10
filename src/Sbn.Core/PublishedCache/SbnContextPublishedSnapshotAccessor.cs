using System;
using Sbn.Cms.Core.Web;

namespace Sbn.Cms.Core.PublishedCache
{
    // TODO: This is a mess. This is a circular reference:
    // IPublishedSnapshotAccessor -> PublishedSnapshotService -> SbnContext -> PublishedSnapshotService -> IPublishedSnapshotAccessor
    // Injecting IPublishedSnapshotAccessor into PublishedSnapshotService seems pretty strange
    // The underlying reason for this mess is because IPublishedContent is both a service and a model.
    // Until that is fixed, IPublishedContent will need to have a IPublishedSnapshotAccessor
    public class SbnContextPublishedSnapshotAccessor : IPublishedSnapshotAccessor
    {
        private readonly ISbnContextAccessor _sbnContextAccessor;

        public SbnContextPublishedSnapshotAccessor(ISbnContextAccessor sbnContextAccessor)
        {
            _sbnContextAccessor = sbnContextAccessor;
        }

        public IPublishedSnapshot PublishedSnapshot
        {
            get
            {
                if (!_sbnContextAccessor.TryGetSbnContext(out var sbnContext))
                {
                    return null;
                }
                return sbnContext.PublishedSnapshot;
            }

            set => throw new NotSupportedException(); // not ok to set
        }

        public bool TryGetPublishedSnapshot(out IPublishedSnapshot publishedSnapshot)
        {
            if (!_sbnContextAccessor.TryGetSbnContext(out var sbnContext))
            {
                publishedSnapshot = null;
                return false;
            }
            publishedSnapshot = sbnContext.PublishedSnapshot;

            return publishedSnapshot is not null;
        }
    }
}
