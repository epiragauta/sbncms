// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Linq;
using NUnit.Framework;
using Sbn.Cms.Core.Cache;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Core.Cache
{
    [TestFixture]
    public class ObjectAppCacheTests : RuntimeAppCacheTests
    {
        private ObjectCacheAppCache _provider;

        protected override int GetTotalItemCount => _provider.MemoryCache.Count();

        public override void Setup()
        {
            base.Setup();
            _provider = new ObjectCacheAppCache();
        }

        internal override IAppCache AppCache => _provider;

        internal override IAppPolicyCache AppPolicyCache => _provider;
    }
}
