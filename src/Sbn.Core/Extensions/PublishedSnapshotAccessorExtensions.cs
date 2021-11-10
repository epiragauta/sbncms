using System;
using Sbn.Cms.Core.PublishedCache;

namespace Sbn.Extensions
{
    public static class PublishedSnapshotAccessorExtensions
    {
        public static IPublishedSnapshot GetRequiredPublishedSnapshot(this IPublishedSnapshotAccessor publishedSnapshotAccessor)
        {
            if (publishedSnapshotAccessor.TryGetPublishedSnapshot(out var publishedSnapshot))
            {
                return publishedSnapshot;
            }
            throw new InvalidOperationException("Wasn't possible to a get a valid Snapshot");
        }
    }
}
