// Copyright (c) Sbn.
// See LICENSE for more details.

using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Sbn.Cms.Core;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.Hosting;
using Sbn.Cms.Core.Routing;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Web;
using Sbn.Cms.Tests.UnitTests.TestHelpers;
using Sbn.Cms.Web.BackOffice.Controllers;
using Sbn.Cms.Web.BackOffice.Security;
using Sbn.Extensions;
using Constants = Sbn.Cms.Core.Constants;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Web.BackOffice.Security
{
    [TestFixture]
    public class BackOfficeCookieManagerTests
    {
        [Test]
        public void ShouldAuthenticateRequest_When_Not_Configured()
        {
            var globalSettings = new GlobalSettings();

            IRuntimeState runtime = Mock.Of<IRuntimeState>(x => x.Level == RuntimeLevel.Install);
            var mgr = new BackOfficeCookieManager(
                Mock.Of<ISbnContextAccessor>(),
                runtime,
                new SbnRequestPaths(Options.Create(globalSettings), TestHelper.GetHostingEnvironment()),
                Mock.Of<IBasicAuthService>());

            var result = mgr.ShouldAuthenticateRequest("/sbn");

            Assert.IsFalse(result);
        }

        [Test]
        public void ShouldAuthenticateRequest_When_Configured()
        {
            var globalSettings = new GlobalSettings();

            IRuntimeState runtime = Mock.Of<IRuntimeState>(x => x.Level == RuntimeLevel.Run);
            var mgr = new BackOfficeCookieManager(
                Mock.Of<ISbnContextAccessor>(),
                runtime,
                new SbnRequestPaths(
                    Options.Create(globalSettings),
                    Mock.Of<IHostingEnvironment>(x => x.ApplicationVirtualPath == "/" && x.ToAbsolute(globalSettings.SbnPath) == "/sbn")),
                Mock.Of<IBasicAuthService>());

            var result = mgr.ShouldAuthenticateRequest("/sbn");

            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldAuthenticateRequest_Is_Back_Office()
        {
            var globalSettings = new GlobalSettings();

            IRuntimeState runtime = Mock.Of<IRuntimeState>(x => x.Level == RuntimeLevel.Run);

            GenerateAuthPaths(out var remainingTimeoutSecondsPath, out var isAuthPath);

            var mgr = new BackOfficeCookieManager(
                Mock.Of<ISbnContextAccessor>(),
                runtime,
                new SbnRequestPaths(
                    Options.Create(globalSettings),
                    Mock.Of<IHostingEnvironment>(x => x.ApplicationVirtualPath == "/" && x.ToAbsolute(globalSettings.SbnPath) == "/sbn" && x.ToAbsolute(Constants.SystemDirectories.Install) == "/install")),
                Mock.Of<IBasicAuthService>());

            var result = mgr.ShouldAuthenticateRequest(remainingTimeoutSecondsPath);
            Assert.IsTrue(result);

            result = mgr.ShouldAuthenticateRequest(isAuthPath);
            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldAuthenticateRequest_Not_Back_Office()
        {
            var globalSettings = new GlobalSettings();

            IRuntimeState runtime = Mock.Of<IRuntimeState>(x => x.Level == RuntimeLevel.Run);

            var mgr = new BackOfficeCookieManager(
                Mock.Of<ISbnContextAccessor>(),
                runtime,
                new SbnRequestPaths(
                    Options.Create(globalSettings),
                    Mock.Of<IHostingEnvironment>(x => x.ApplicationVirtualPath == "/" && x.ToAbsolute(globalSettings.SbnPath) == "/sbn" && x.ToAbsolute(Constants.SystemDirectories.Install) == "/install")),
                Mock.Of<IBasicAuthService>());

            var result = mgr.ShouldAuthenticateRequest("/notbackoffice");
            Assert.IsFalse(result);
            result = mgr.ShouldAuthenticateRequest("/sbn/api/notbackoffice");
            Assert.IsFalse(result);
            result = mgr.ShouldAuthenticateRequest("/sbn/surface/notbackoffice");
            Assert.IsFalse(result);
        }

        private void GenerateAuthPaths(out string remainingTimeoutSecondsPath, out string isAuthPath)
        {
            var controllerName = ControllerExtensions.GetControllerName<AuthenticationController>();

            // this path is not a back office request even though it's in the same controller - it's a 'special' endpoint
            var rPath = remainingTimeoutSecondsPath = $"/sbn/{Constants.Web.Mvc.BackOfficePathSegment}/{Constants.Web.Mvc.BackOfficeApiArea}/{controllerName}/{nameof(AuthenticationController.GetRemainingTimeoutSeconds)}".ToLower();

            // this is on the same controller but is considered a back office request
            var aPath = isAuthPath = $"/sbn/{Constants.Web.Mvc.BackOfficePathSegment}/{Constants.Web.Mvc.BackOfficeApiArea}/{controllerName}/{nameof(AuthenticationController.IsAuthenticated)}".ToLower();
        }
    }
}
