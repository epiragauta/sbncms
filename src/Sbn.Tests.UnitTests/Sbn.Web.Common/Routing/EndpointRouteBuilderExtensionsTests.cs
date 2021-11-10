// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NUnit.Framework;
using Sbn.Extensions;
using static Sbn.Cms.Core.Constants.Web.Routing;
using Constants = Sbn.Cms.Core.Constants;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Web.Common.Routing
{
    [TestFixture]
    public class EndpointRouteBuilderExtensionsTests
    {
        [TestCase("sbn", Constants.Web.Mvc.BackOfficeApiArea, "test", null, true)]
        [TestCase("sbn", Constants.Web.Mvc.BackOfficeApiArea, "test", "GetStuff", true)]
        [TestCase("sbn", Constants.Web.Mvc.BackOfficeApiArea, null, null, true)]
        [TestCase("sbn", Constants.Web.Mvc.BackOfficeApiArea, null, "GetStuff", true)]
        [TestCase("sbn", Constants.Web.Mvc.BackOfficeApiArea, "test", null, false)]
        [TestCase("sbn", Constants.Web.Mvc.BackOfficeApiArea, "test", "GetStuff", false)]
        [TestCase("sbn", Constants.Web.Mvc.BackOfficeApiArea, null, null, false)]
        [TestCase("sbn", Constants.Web.Mvc.BackOfficeApiArea, null, "GetStuff", false)]
        [TestCase("sbn", null, "test", null, true)]
        [TestCase("sbn", null, "test", "GetStuff", true)]
        [TestCase("sbn", null, null, null, true)]
        [TestCase("sbn", null, null, "GetStuff", true)]
        [TestCase("sbn", null, "test", null, false)]
        [TestCase("sbn", null, "test", "GetStuff", false)]
        [TestCase("sbn", null, null, null, false)]
        [TestCase("sbn", null, null, "GetStuff", false)]
        public void MapSbnRoute(string sbnPath, string area, string prefix, string defaultAction, bool includeControllerName)
        {
            var endpoints = new TestRouteBuilder();
            endpoints.MapSbnRoute<Testing1Controller>(sbnPath, area, prefix, defaultAction, includeControllerName);

            EndpointDataSource route = endpoints.DataSources.First();
            var endpoint = (RouteEndpoint)route.Endpoints[0];

            string controllerName = ControllerExtensions.GetControllerName<Testing1Controller>();
            string controllerNamePattern = controllerName.ToLowerInvariant();

            if (includeControllerName)
            {
                if (prefix.IsNullOrWhiteSpace())
                {
                    Assert.AreEqual($"{sbnPath}/{controllerNamePattern}/{{action}}/{{id?}}", endpoint.RoutePattern.RawText);
                }
                else
                {
                    Assert.AreEqual($"{sbnPath}/{prefix}/{controllerNamePattern}/{{action}}/{{id?}}", endpoint.RoutePattern.RawText);
                }
            }
            else
            {
                if (prefix.IsNullOrWhiteSpace())
                {
                    Assert.AreEqual($"{sbnPath}/{{action}}/{{id?}}", endpoint.RoutePattern.RawText);
                }
                else
                {
                    Assert.AreEqual($"{sbnPath}/{prefix}/{{action}}/{{id?}}", endpoint.RoutePattern.RawText);
                }
            }

            if (!area.IsNullOrWhiteSpace())
            {
                Assert.AreEqual(area, endpoint.RoutePattern.Defaults[AreaToken]);
            }

            if (!defaultAction.IsNullOrWhiteSpace())
            {
                Assert.AreEqual(defaultAction, endpoint.RoutePattern.Defaults["action"]);
            }

            Assert.AreEqual(controllerName, endpoint.RoutePattern.Defaults[ControllerToken]);
        }

        [TestCase("sbn", Constants.Web.Mvc.BackOfficeApiArea, true, null)]
        [TestCase("sbn", Constants.Web.Mvc.BackOfficeApiArea, true, "GetStuff")]
        [TestCase("sbn", Constants.Web.Mvc.BackOfficeApiArea, false, null)]
        [TestCase("sbn", Constants.Web.Mvc.BackOfficeApiArea, false, "GetStuff")]
        [TestCase("sbn", null, true, null)]
        [TestCase("sbn", null, true, "GetStuff")]
        [TestCase("sbn", null, false, null)]
        [TestCase("sbn", null, false, "GetStuff")]
        public void MapSbnApiRoute(string sbnPath, string area, bool isBackOffice, string defaultAction)
        {
            var endpoints = new TestRouteBuilder();
            endpoints.MapSbnApiRoute<Testing1Controller>(sbnPath, area, isBackOffice, defaultAction);

            EndpointDataSource route = endpoints.DataSources.First();
            var endpoint = (RouteEndpoint)route.Endpoints[0];

            string controllerName = ControllerExtensions.GetControllerName<Testing1Controller>();
            string controllerNamePattern = controllerName.ToLowerInvariant();
            string areaPattern = area?.ToLowerInvariant();

            if (isBackOffice)
            {
                if (area.IsNullOrWhiteSpace())
                {
                    Assert.AreEqual($"{sbnPath}/backoffice/api/{controllerNamePattern}/{{action}}/{{id?}}", endpoint.RoutePattern.RawText);
                }
                else
                {
                    Assert.AreEqual($"{sbnPath}/backoffice/{areaPattern}/{controllerNamePattern}/{{action}}/{{id?}}", endpoint.RoutePattern.RawText);
                }
            }
            else
            {
                if (area.IsNullOrWhiteSpace())
                {
                    Assert.AreEqual($"{sbnPath}/api/{controllerNamePattern}/{{action}}/{{id?}}", endpoint.RoutePattern.RawText);
                }
                else
                {
                    Assert.AreEqual($"{sbnPath}/{areaPattern}/{controllerNamePattern}/{{action}}/{{id?}}", endpoint.RoutePattern.RawText);
                }
            }

            if (!area.IsNullOrWhiteSpace())
            {
                Assert.AreEqual(area, endpoint.RoutePattern.Defaults[AreaToken]);
            }

            if (!defaultAction.IsNullOrWhiteSpace())
            {
                Assert.AreEqual(defaultAction, endpoint.RoutePattern.Defaults["action"]);
            }

            Assert.AreEqual(controllerName, endpoint.RoutePattern.Defaults[ControllerToken]);
        }

        private class Testing1Controller : ControllerBase
        {
        }
    }
}
