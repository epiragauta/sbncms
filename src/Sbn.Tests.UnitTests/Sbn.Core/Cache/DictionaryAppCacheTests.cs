// Copyright (c) Sbn.
// See LICENSE for more details.

using NUnit.Framework;
using Sbn.Cms.Core.Cache;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Core.Cache
{
    [TestFixture]
    public class DictionaryAppCacheTests : AppCacheTests
    {
        private DictionaryAppCache _appCache;

        public override void Setup()
        {
            base.Setup();
            _appCache = new DictionaryAppCache();
        }

        internal override IAppCache AppCache => _appCache;

        protected override int GetTotalItemCount => _appCache.Count;
    }
}
