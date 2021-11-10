using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Sbn.Cms.Core.Cache;
using Sbn.Cms.Core.Logging;
using Sbn.Cms.Core.Routing;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Web;
using Sbn.Cms.Infrastructure.Persistence;
using Sbn.Cms.Tests.Integration.TestServerTest;
using Sbn.Cms.Web.Common.Attributes;
using Sbn.Cms.Web.Website.Controllers;
using Sbn.Extensions;

namespace Sbn.Cms.Tests.Integration.Sbn.Web.Website.Routing
{
    [TestFixture]
    public class SurfaceControllerTests : SbnTestServerTestBase
    {
        [Test]
        public async Task Auto_Routes_For_Default_Action()
        {
            string url = PrepareSurfaceControllerUrl<TestSurfaceController>(x => x.Index());

            // Act
            HttpResponseMessage response = await Client.GetAsync(url);

            string body = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task Auto_Routes_For_Custom_Action()
        {
            string url = PrepareSurfaceControllerUrl<TestSurfaceController>(x => x.News());

            // Act
            HttpResponseMessage response = await Client.GetAsync(url);

            string body = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Test]
        public async Task Plugin_Controller_Routes_By_Area()
        {
            // Create URL manually, because PrepareSurfaceController URl will prepare whatever the controller is routed as
            Type controllerType = typeof(TestPluginController);
            var pluginAttribute = CustomAttributeExtensions.GetCustomAttribute<PluginControllerAttribute>(controllerType, false);
            var controllerName = ControllerExtensions.GetControllerName(controllerType);
            string url = $"/sbn/{pluginAttribute?.AreaName}/{controllerName}";
            PrepareUrl(url);

            HttpResponseMessage response = await Client.GetAsync(url);

            string body = await response.Content.ReadAsStringAsync();

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        }
    }

    // Test controllers must be non-nested, else we need to jump through some hoops with custom
    // IApplicationFeatureProvider<ControllerFeature>
    // For future notes if we want this, some example code of this is here
    // https://tpodolak.com/blog/2020/06/22/asp-net-core-adding-controllers-directly-integration-tests/
    public class TestSurfaceController : SurfaceController
    {
        public TestSurfaceController(ISbnContextAccessor sbnContextAccessor, ISbnDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider)
            : base(sbnContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
        }

        public IActionResult Index() => Ok();

        public IActionResult News() => NoContent();
    }

    [PluginController("TestArea")]
    public class TestPluginController : SurfaceController
    {
        public TestPluginController(ISbnContextAccessor sbnContextAccessor, ISbnDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider) : base(sbnContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
        }

        public IActionResult Index() => Ok();
    }
}
