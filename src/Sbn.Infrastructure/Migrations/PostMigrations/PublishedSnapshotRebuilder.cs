using Sbn.Cms.Core.Cache;
using Sbn.Cms.Core.PublishedCache;
using Sbn.Extensions;

namespace Sbn.Cms.Infrastructure.Migrations.PostMigrations
{
    /// <summary>
    /// Implements <see cref="IPublishedSnapshotRebuilder"/> in Sbn.Web (rebuilding).
    /// </summary>
    public class PublishedSnapshotRebuilder : IPublishedSnapshotRebuilder
    {
        private readonly IPublishedSnapshotService _publishedSnapshotService;
        private readonly DistributedCache _distributedCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="PublishedSnapshotRebuilder"/> class.
        /// </summary>
        public PublishedSnapshotRebuilder(IPublishedSnapshotService publishedSnapshotService, DistributedCache distributedCache)
        {
            _publishedSnapshotService = publishedSnapshotService;
            _distributedCache = distributedCache;
        }

        /// <inheritdoc />
        public void Rebuild()
        {
            _publishedSnapshotService.Rebuild();
            _distributedCache.RefreshAllPublishedSnapshot();
        }
    }
}
