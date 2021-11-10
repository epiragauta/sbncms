using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.Routing;
using Sbn.Cms.Tests.Common.Testing;
using Sbn.Cms.Web.Common.AspNetCore;
using IHostingEnvironment = Sbn.Cms.Core.Hosting.IHostingEnvironment;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Core.Routing
{
    [TestFixture]
    public class SbnRequestPathsTests
    {
        private IWebHostEnvironment _hostEnvironment;
        private GlobalSettings _globalSettings;

        [OneTimeSetUp]
        public void Setup()
        {
            _hostEnvironment = Mock.Of<IWebHostEnvironment>();
            _globalSettings = new GlobalSettings();
        }

        private IHostingEnvironment CreateHostingEnvironment(string virtualPath = "")
        {
            var hostingSettings = new HostingSettings { ApplicationVirtualPath = virtualPath };
            var webRoutingSettings = new WebRoutingSettings();
            var mockedOptionsMonitorOfHostingSettings = Mock.Of<IOptionsMonitor<HostingSettings>>(x => x.CurrentValue == hostingSettings);
            var mockedOptionsMonitorOfWebRoutingSettings = Mock.Of<IOptionsMonitor<WebRoutingSettings>>(x => x.CurrentValue == webRoutingSettings);

            return new TestHostingEnvironment(
                mockedOptionsMonitorOfHostingSettings,
                mockedOptionsMonitorOfWebRoutingSettings,
                _hostEnvironment);
        }

        [TestCase("/favicon.ico", true)]
        [TestCase("/sbn_client/Tree/treeIcons.css", true)]
        [TestCase("/sbn_client/Tree/Themes/sbn/style.css?cdv=37", true)]
        [TestCase("/base/somebasehandler", false)]
        [TestCase("/", false)]
        [TestCase("/home.aspx", true)] // has ext, assume client side
        [TestCase("http://www.domain.com/Sbn/test/test.aspx", true)] // has ext, assume client side
        [TestCase("http://www.domain.com/sbn/test/test.js", true)]
        public void Is_Client_Side_Request(string url, bool assert)
        {
            IHostingEnvironment hostingEnvironment = CreateHostingEnvironment();
            var sbnRequestPaths = new SbnRequestPaths(Options.Create(_globalSettings), hostingEnvironment);

            var uri = new Uri("http://test.com" + url);
            var result = sbnRequestPaths.IsClientSideRequest(uri.AbsolutePath);
            Assert.AreEqual(assert, result);
        }

        [Test]
        public void Is_Client_Side_Request_InvalidPath_ReturnFalse()
        {
            IHostingEnvironment hostingEnvironment = CreateHostingEnvironment();
            var sbnRequestPaths = new SbnRequestPaths(Options.Create(_globalSettings), hostingEnvironment);

            // This URL is invalid. Default to false when the extension cannot be determined
            var uri = new Uri("http://test.com/installing-modules+foobar+\"yipee\"");
            var result = sbnRequestPaths.IsClientSideRequest(uri.AbsolutePath);
            Assert.AreEqual(false, result);
        }

        [TestCase("http://www.domain.com/sbn/preview/frame?id=1234", "", true)]
        [TestCase("http://www.domain.com/sbn", "", true)]
        [TestCase("http://www.domain.com/Sbn/", "", true)]
        [TestCase("http://www.domain.com/sbn/default.aspx", "", true)]
        [TestCase("http://www.domain.com/sbn/test/test", "", false)]
        [TestCase("http://www.domain.com/sbn/test/test/test", "", false)]
        [TestCase("http://www.domain.com/umbrac", "", false)]
        [TestCase("http://www.domain.com/test", "", false)]
        [TestCase("http://www.domain.com/test/sbn", "", false)]
        [TestCase("http://www.domain.com/Sbn/Backoffice/blah", "", true)]
        [TestCase("http://www.domain.com/Sbn/anything", "", true)]
        [TestCase("http://www.domain.com/Sbn/anything/", "", true)]
        [TestCase("http://www.domain.com/Sbn/surface/blah", "", false)]
        [TestCase("http://www.domain.com/sbn/api/blah", "", false)]
        [TestCase("http://www.domain.com/myvdir/sbn/api/blah", "myvdir", false)]
        [TestCase("http://www.domain.com/MyVdir/sbn/api/blah", "/myvdir", false)]
        [TestCase("http://www.domain.com/MyVdir/Sbn/", "myvdir", true)]
        public void Is_Back_Office_Request(string input, string virtualPath, bool expected)
        {
            var source = new Uri(input);
            var hostingEnvironment = CreateHostingEnvironment(virtualPath);
            var sbnRequestPaths = new SbnRequestPaths(Options.Create(_globalSettings), hostingEnvironment);
            Assert.AreEqual(expected, sbnRequestPaths.IsBackOfficeRequest(source.AbsolutePath));
        }

        [TestCase("http://www.domain.com/install", true)]
        [TestCase("http://www.domain.com/Install/", true)]
        [TestCase("http://www.domain.com/install/default.aspx", true)]
        [TestCase("http://www.domain.com/install/test/test", true)]
        [TestCase("http://www.domain.com/Install/test/test.aspx", true)]
        [TestCase("http://www.domain.com/install/test/test.js", true)]
        [TestCase("http://www.domain.com/instal", false)]
        [TestCase("http://www.domain.com/sbn", false)]
        [TestCase("http://www.domain.com/sbn/sbn", false)]
        public void Is_Installer_Request(string input, bool expected)
        {
            var source = new Uri(input);
            var hostingEnvironment = CreateHostingEnvironment();
            var sbnRequestPaths = new SbnRequestPaths(Options.Create(_globalSettings), hostingEnvironment);
            Assert.AreEqual(expected, sbnRequestPaths.IsInstallerRequest(source.AbsolutePath));
        }
    }
}
