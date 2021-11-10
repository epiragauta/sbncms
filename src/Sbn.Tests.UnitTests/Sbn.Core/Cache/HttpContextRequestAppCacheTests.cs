// Copyright (c) Sbn.
// See LICENSE for more details.

using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using Sbn.Cms.Core.Cache;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Core.Cache
{
    [TestFixture]
    public class HttpContextRequestAppCacheTests : AppCacheTests
    {
        private HttpContextRequestAppCache _appCache;
        private IHttpContextAccessor _httpContextAccessor;

        public override void Setup()
        {
            base.Setup();
            var httpContext = new DefaultHttpContext();

            _httpContextAccessor = Mock.Of<IHttpContextAccessor>(x => x.HttpContext == httpContext);
            _appCache = new HttpContextRequestAppCache(_httpContextAccessor);
        }

        internal override IAppCache AppCache => _appCache;

        protected override int GetTotalItemCount => _httpContextAccessor.HttpContext.Items.Count;
    }
}
