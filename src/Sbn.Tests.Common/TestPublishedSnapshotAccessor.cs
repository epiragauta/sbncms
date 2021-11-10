// Copyright (c) Sbn.
// See LICENSE for more details.

using Sbn.Cms.Core.PublishedCache;

namespace Sbn.Cms.Tests.Common
{
    public class TestPublishedSnapshotAccessor : IPublishedSnapshotAccessor
    {
        public bool TryGetPublishedSnapshot(out IPublishedSnapshot publishedSnapshot)
        {
            publishedSnapshot = null;
            return false;
        }
    }
}
