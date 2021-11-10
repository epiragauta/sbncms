// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Sbn.Cms.Core;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.Hosting;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Web.BackOffice.Controllers;
using Sbn.Cms.Web.BackOffice.Routing;
using Sbn.Cms.Web.Common.Attributes;
using Sbn.Cms.Web.Common.Controllers;
using Sbn.Extensions;
using Constants = Sbn.Cms.Core.Constants;
using static Sbn.Cms.Core.Constants.Web.Routing;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Web.Common.Routing
{
    [TestFixture]
    public class BackOfficeAreaRoutesTests
    {

        [TestCase(RuntimeLevel.BootFailed)]
        [TestCase(RuntimeLevel.Unknown)]
        [TestCase(RuntimeLevel.Boot)]
        public void RuntimeState_No_Routes(RuntimeLevel level)
        {
            BackOfficeAreaRoutes routes = GetBackOfficeAreaRoutes(level);
            var endpoints = new TestRouteBuilder();
            routes.CreateRoutes(endpoints);

            Assert.AreEqual(0, endpoints.DataSources.Count);
        }

        [Test]
        [TestCase(RuntimeLevel.Run)]
        [TestCase(RuntimeLevel.Upgrade)]
        [TestCase(RuntimeLevel.Install)]
        public void RuntimeState_All_Routes(RuntimeLevel level)
        {
            BackOfficeAreaRoutes routes = GetBackOfficeAreaRoutes(level);
            var endpoints = new TestRouteBuilder();
            routes.CreateRoutes(endpoints);

            Assert.AreEqual(1, endpoints.DataSources.Count);
            EndpointDataSource route = endpoints.DataSources.First();
            Assert.AreEqual(3, route.Endpoints.Count);

            AssertMinimalBackOfficeRoutes(route);

            var endpoint4 = (RouteEndpoint)route.Endpoints[2];
            string apiControllerName = ControllerExtensions.GetControllerName<Testing1Controller>();
            Assert.AreEqual($"sbn/backoffice/api/{apiControllerName.ToLowerInvariant()}/{{action}}/{{id?}}", endpoint4.RoutePattern.RawText);
            Assert.IsFalse(endpoint4.RoutePattern.Defaults.ContainsKey(AreaToken));
            Assert.IsFalse(endpoint4.RoutePattern.Defaults.ContainsKey(ActionToken));
            Assert.AreEqual(apiControllerName, endpoint4.RoutePattern.Defaults[ControllerToken]);
        }

        private void AssertMinimalBackOfficeRoutes(EndpointDataSource route)
        {
            var endpoint1 = (RouteEndpoint)route.Endpoints[0];
            Assert.AreEqual($"sbn/{{action}}/{{id?}}", endpoint1.RoutePattern.RawText);
            Assert.AreEqual(Constants.Web.Mvc.BackOfficeArea, endpoint1.RoutePattern.Defaults[AreaToken]);
            Assert.AreEqual("Default", endpoint1.RoutePattern.Defaults[ActionToken]);
            Assert.AreEqual(ControllerExtensions.GetControllerName<BackOfficeController>(), endpoint1.RoutePattern.Defaults[ControllerToken]);
            Assert.AreEqual(endpoint1.RoutePattern.Defaults[AreaToken], typeof(BackOfficeController).GetCustomAttribute<AreaAttribute>(false).RouteValue);

            var endpoint2 = (RouteEndpoint)route.Endpoints[1];
            string controllerName = ControllerExtensions.GetControllerName<AuthenticationController>();
            Assert.AreEqual($"sbn/backoffice/{Constants.Web.Mvc.BackOfficeApiArea.ToLowerInvariant()}/{controllerName.ToLowerInvariant()}/{{action}}/{{id?}}", endpoint2.RoutePattern.RawText);
            Assert.AreEqual(Constants.Web.Mvc.BackOfficeApiArea, endpoint2.RoutePattern.Defaults[AreaToken]);
            Assert.IsFalse(endpoint2.RoutePattern.Defaults.ContainsKey(ActionToken));
            Assert.AreEqual(controllerName, endpoint2.RoutePattern.Defaults[ControllerToken]);
            Assert.AreEqual(endpoint1.RoutePattern.Defaults[AreaToken], typeof(BackOfficeController).GetCustomAttribute<AreaAttribute>(false).RouteValue);
        }

        private BackOfficeAreaRoutes GetBackOfficeAreaRoutes(RuntimeLevel level)
        {
            var globalSettings = new GlobalSettings();
            var routes = new BackOfficeAreaRoutes(
                Options.Create(globalSettings),
                Mock.Of<IHostingEnvironment>(x => x.ToAbsolute(It.IsAny<string>()) == "/sbn" && x.ApplicationVirtualPath == string.Empty),
                Mock.Of<IRuntimeState>(x => x.Level == level),
                new SbnApiControllerTypeCollection(() => new[] { typeof(Testing1Controller) }));

            return routes;
        }

        [IsBackOffice]
        private class Testing1Controller : SbnApiController
        {
        }
    }
}
