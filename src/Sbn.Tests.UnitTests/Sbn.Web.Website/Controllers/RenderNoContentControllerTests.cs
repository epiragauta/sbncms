// Copyright (c) Sbn.
// See LICENSE for more details.

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.IO;
using Sbn.Cms.Core.Web;
using Sbn.Cms.Tests.Common;
using Sbn.Cms.Web.Website.Controllers;
using Sbn.Cms.Web.Website.Models;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Web.Website.Controllers
{
    [TestFixture]
    public class RenderNoContentControllerTests
    {
        [Test]
        public void Redirects_To_Root_When_Content_Published()
        {
            var mockSbnContext = new Mock<ISbnContext>();
            mockSbnContext.Setup(x => x.Content.HasContent()).Returns(true);
            var mockIOHelper = new Mock<IIOHelper>();
            var controller = new RenderNoContentController(new TestSbnContextAccessor(mockSbnContext.Object), mockIOHelper.Object, Options.Create(new GlobalSettings()));

            var result = controller.Index() as RedirectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("~/", result.Url);
        }

        [Test]
        public void Renders_View_When_No_Content_Published()
        {
            const string SbnPathSetting = "~/sbn";
            const string SbnPath = "/sbn";
            const string ViewPath = "~/config/splashes/NoNodes.cshtml";
            var mockSbnContext = new Mock<ISbnContext>();
            mockSbnContext.Setup(x => x.Content.HasContent()).Returns(false);
            var mockIOHelper = new Mock<IIOHelper>();
            mockIOHelper.Setup(x => x.ResolveUrl(It.Is<string>(y => y == SbnPathSetting))).Returns(SbnPath);

            IOptions<GlobalSettings> globalSettings = Options.Create(new GlobalSettings()
            {
                SbnPath = SbnPathSetting,
                NoNodesViewPath = ViewPath,
            });
            var controller = new RenderNoContentController(new TestSbnContextAccessor(mockSbnContext.Object), mockIOHelper.Object, globalSettings);

            var result = controller.Index() as ViewResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(ViewPath, result.ViewName);

            var model = result.Model as NoNodesViewModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(SbnPath, model.SbnPath);
        }
    }
}
