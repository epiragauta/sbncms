// Copyright (c) Sbn.
// See LICENSE for more details.

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.Hosting;
using Sbn.Cms.Core.PublishedCache;
using Sbn.Cms.Core.Routing;
using Sbn.Cms.Core.Web;
using Sbn.Cms.Tests.Common;
using Sbn.Cms.Web.Common.AspNetCore;
using Sbn.Cms.Web.Common.SbnContext;

namespace Sbn.Cms.Tests.UnitTests.TestHelpers.Objects
{
    /// <summary>
    /// Simplify creating test SbnContext's
    /// </summary>
    public class TestSbnContextFactory
    {
        public static ISbnContextFactory Create(
            GlobalSettings globalSettings = null,
            ISbnContextAccessor sbnContextAccessor = null,
            IHttpContextAccessor httpContextAccessor = null,
            IPublishedUrlProvider publishedUrlProvider = null)
        {
            if (globalSettings == null)
            {
                globalSettings = new GlobalSettings();
            }

            if (sbnContextAccessor == null)
            {
                sbnContextAccessor = new TestSbnContextAccessor();
            }

            if (httpContextAccessor == null)
            {
                httpContextAccessor = Mock.Of<IHttpContextAccessor>();
            }

            if (publishedUrlProvider == null)
            {
                publishedUrlProvider = Mock.Of<IPublishedUrlProvider>();
            }

            var contentCache = new Mock<IPublishedContentCache>();
            var mediaCache = new Mock<IPublishedMediaCache>();
            var snapshot = new Mock<IPublishedSnapshot>();
            snapshot.Setup(x => x.Content).Returns(contentCache.Object);
            snapshot.Setup(x => x.Media).Returns(mediaCache.Object);
            var snapshotService = new Mock<IPublishedSnapshotService>();
            snapshotService.Setup(x => x.CreatePublishedSnapshot(It.IsAny<string>())).Returns(snapshot.Object);

            IHostingEnvironment hostingEnvironment = TestHelper.GetHostingEnvironment();

            var sbnContextFactory = new SbnContextFactory(
                sbnContextAccessor,
                snapshotService.Object,
                new SbnRequestPaths(Options.Create(globalSettings), hostingEnvironment),
                hostingEnvironment,
                new UriUtility(hostingEnvironment),
                new AspNetCoreCookieManager(httpContextAccessor),
                httpContextAccessor);

            return sbnContextFactory;
        }
    }
}
