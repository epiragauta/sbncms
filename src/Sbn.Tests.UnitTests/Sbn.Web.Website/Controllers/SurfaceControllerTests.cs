// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;
using Sbn.Cms.Core;
using Sbn.Cms.Core.Cache;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.Hosting;
using Sbn.Cms.Core.Models.PublishedContent;
using Sbn.Cms.Core.PublishedCache;
using Sbn.Cms.Core.Routing;
using Sbn.Cms.Core.Security;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Web;
using Sbn.Cms.Tests.Common;
using Sbn.Cms.Tests.Common.Testing;
using Sbn.Cms.Tests.UnitTests.TestHelpers.Objects;
using Sbn.Cms.Web.Common.Routing;
using Sbn.Cms.Web.Website.Controllers;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Web.Website.Controllers
{
    [TestFixture]
    [SbnTest(WithApplication = true)]
    public class SurfaceControllerTests
    {
        private ISbnContextAccessor _sbnContextAccessor;

        [SetUp]
        public void SetUp() => _sbnContextAccessor = new TestSbnContextAccessor();

        [Test]
        public void Can_Construct_And_Get_Result()
        {
            IHostingEnvironment hostingEnvironment = Mock.Of<IHostingEnvironment>();
            IBackOfficeSecurityAccessor backofficeSecurityAccessor = Mock.Of<IBackOfficeSecurityAccessor>();
            Mock.Get(backofficeSecurityAccessor).Setup(x => x.BackOfficeSecurity).Returns(Mock.Of<IBackOfficeSecurity>());
            var globalSettings = new GlobalSettings();

            var sbnContextFactory = TestSbnContextFactory.Create(globalSettings, _sbnContextAccessor);

            SbnContextReference sbnContextReference = sbnContextFactory.EnsureSbnContext();
            ISbnContext sbnContext = sbnContextReference.SbnContext;

            var sbnContextAccessor = new TestSbnContextAccessor(sbnContext);

            var ctrl = new TestSurfaceController(sbnContextAccessor, Mock.Of<IPublishedContentQuery>(), Mock.Of<IPublishedUrlProvider>());

            IActionResult result = ctrl.Index();

            Assert.IsNotNull(result);
        }

        [Test]
        public void Sbn_Context_Not_Null()
        {
            var globalSettings = new GlobalSettings();
            IHostingEnvironment hostingEnvironment = Mock.Of<IHostingEnvironment>();
            IBackOfficeSecurityAccessor backofficeSecurityAccessor = Mock.Of<IBackOfficeSecurityAccessor>();
            Mock.Get(backofficeSecurityAccessor).Setup(x => x.BackOfficeSecurity).Returns(Mock.Of<IBackOfficeSecurity>());
            var sbnContextFactory = TestSbnContextFactory.Create(globalSettings, _sbnContextAccessor);

            SbnContextReference sbnContextReference = sbnContextFactory.EnsureSbnContext();
            ISbnContext umbCtx = sbnContextReference.SbnContext;

            var sbnContextAccessor = new TestSbnContextAccessor(umbCtx);

            var ctrl = new TestSurfaceController(sbnContextAccessor, Mock.Of<IPublishedContentQuery>(), Mock.Of<IPublishedUrlProvider>());

            Assert.IsNotNull(ctrl.SbnContext);
        }

        [Test]
        public void Can_Lookup_Content()
        {
            var publishedSnapshot = new Mock<IPublishedSnapshot>();
            publishedSnapshot.Setup(x => x.Members).Returns(Mock.Of<IPublishedMemberCache>());
            var content = new Mock<IPublishedContent>();
            content.Setup(x => x.Id).Returns(2);
            IBackOfficeSecurityAccessor backofficeSecurityAccessor = Mock.Of<IBackOfficeSecurityAccessor>();
            Mock.Get(backofficeSecurityAccessor).Setup(x => x.BackOfficeSecurity).Returns(Mock.Of<IBackOfficeSecurity>());
            var publishedSnapshotService = new Mock<IPublishedSnapshotService>();
            IHostingEnvironment hostingEnvironment = Mock.Of<IHostingEnvironment>();
            var globalSettings = new GlobalSettings();

            var sbnContextFactory = TestSbnContextFactory.Create(globalSettings, _sbnContextAccessor);

            SbnContextReference sbnContextReference = sbnContextFactory.EnsureSbnContext();
            ISbnContext sbnContext = sbnContextReference.SbnContext;

            var sbnContextAccessor = new TestSbnContextAccessor(sbnContext);

            IPublishedContentQuery publishedContentQuery = Mock.Of<IPublishedContentQuery>(query => query.Content(2) == content.Object);

            var ctrl = new TestSurfaceController(sbnContextAccessor, publishedContentQuery, Mock.Of<IPublishedUrlProvider>());
            var result = ctrl.GetContent(2) as PublishedContentResult;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(2, result.Content.Id);
        }

        [Test]
        public void Mock_Current_Page()
        {
            var globalSettings = new GlobalSettings();
            IHostingEnvironment hostingEnvironment = Mock.Of<IHostingEnvironment>();
            IBackOfficeSecurityAccessor backofficeSecurityAccessor = Mock.Of<IBackOfficeSecurityAccessor>();
            Mock.Get(backofficeSecurityAccessor).Setup(x => x.BackOfficeSecurity).Returns(Mock.Of<IBackOfficeSecurity>());
            var sbnContextFactory = TestSbnContextFactory.Create(globalSettings, _sbnContextAccessor);

            SbnContextReference sbnContextReference = sbnContextFactory.EnsureSbnContext();
            ISbnContext sbnContext = sbnContextReference.SbnContext;

            var sbnContextAccessor = new TestSbnContextAccessor(sbnContext);

            IPublishedContent content = Mock.Of<IPublishedContent>(publishedContent => publishedContent.Id == 12345);
            var builder = new PublishedRequestBuilder(sbnContext.CleanedSbnUrl, Mock.Of<IFileService>());
            builder.SetPublishedContent(content);
            IPublishedRequest publishedRequest = builder.Build();

            var routeDefinition = new SbnRouteValues(publishedRequest, null);

            var httpContext = new DefaultHttpContext();
            httpContext.Features.Set(routeDefinition);

            var ctrl = new TestSurfaceController(sbnContextAccessor, Mock.Of<IPublishedContentQuery>(), Mock.Of<IPublishedUrlProvider>())
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                    RouteData = new RouteData()
                }
            };

            var result = ctrl.GetContentFromCurrentPage() as PublishedContentResult;

            Assert.AreEqual(12345, result.Content.Id);
        }

        public class TestSurfaceController : SurfaceController
        {
            private readonly IPublishedContentQuery _publishedContentQuery;

            public TestSurfaceController(ISbnContextAccessor sbnContextAccessor, IPublishedContentQuery publishedContentQuery, IPublishedUrlProvider publishedUrlProvider)
                : base(sbnContextAccessor, null, ServiceContext.CreatePartial(), AppCaches.Disabled, null, publishedUrlProvider) =>
                _publishedContentQuery = publishedContentQuery;

            public IActionResult Index() =>

                // ReSharper disable once Mvc.ViewNotResolved
                View();

            public IActionResult GetContent(int id)
            {
                IPublishedContent content = _publishedContentQuery.Content(id);

                return new PublishedContentResult(content);
            }

            public IActionResult GetContentFromCurrentPage()
            {
                IPublishedContent content = CurrentPage;

                return new PublishedContentResult(content);
            }
        }

        public class PublishedContentResult : IActionResult
        {
            public IPublishedContent Content { get; set; }

            public PublishedContentResult(IPublishedContent content) => Content = content;

            public Task ExecuteResultAsync(ActionContext context) => Task.CompletedTask;
        }
    }
}
