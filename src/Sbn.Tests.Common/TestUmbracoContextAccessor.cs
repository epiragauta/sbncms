// Copyright (c) Sbn.
// See LICENSE for more details.

using Sbn.Cms.Core.Web;

namespace Sbn.Cms.Tests.Common
{
    public class TestSbnContextAccessor : ISbnContextAccessor
    {
        private ISbnContext _sbnContext;

        public TestSbnContextAccessor()
        {
        }

        public TestSbnContextAccessor(ISbnContext sbnContext) => _sbnContext = sbnContext;

        public bool TryGetSbnContext(out ISbnContext sbnContext)
        {
            sbnContext = _sbnContext;
            return sbnContext is not null;
        }

        public void Clear() => _sbnContext = null;
        public void Set(ISbnContext sbnContext) => _sbnContext = sbnContext;
    }
}
