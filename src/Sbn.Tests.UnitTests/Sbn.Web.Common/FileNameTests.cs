// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Sbn.Cms.Core;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.Hosting;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Tests.UnitTests.AutoFixture;
using Sbn.Cms.Web.BackOffice.Controllers;
using Sbn.Cms.Web.BackOffice.Install;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Web.Common
{
    [TestFixture]
    internal class FileNameTests
    {
        private string GetViewName(ViewResult viewResult, string separator = "/")
        {
            var sections = viewResult.ViewName.Split(separator);
            return sections[^1];
        }

        private IEnumerable<string> GetUiFiles(IEnumerable<string> pathFromNetCore)
        {
            var sourceRoot = TestContext.CurrentContext.TestDirectory.Split("Sbn.Tests.UnitTests")[0];
            var pathToFiles = Path.Combine(sourceRoot, "Sbn.Web.UI");
            foreach (var pathSection in pathFromNetCore)
            {
                pathToFiles = Path.Combine(pathToFiles, pathSection);
            }

            return new DirectoryInfo(pathToFiles).GetFiles().Select(f => f.Name).ToArray();
        }

        [Test]
        [AutoMoqData]
        public async Task InstallViewExists(
            [Frozen] IHostingEnvironment hostingEnvironment,
            InstallController sut)
        {
            Mock.Get(hostingEnvironment).Setup(x => x.ToAbsolute(It.IsAny<string>())).Returns("http://localhost/");
            var viewResult = await sut.Index() as ViewResult;
            var fileName = GetViewName(viewResult, Path.DirectorySeparatorChar.ToString());

            IEnumerable<string> views = GetUiFiles(new[] { "sbn", "SbnInstall" });
            Assert.True(views.Contains(fileName), $"Expected {fileName} to exist, but it didn't");
        }

        [Test]
        [AutoMoqData]
        public void PreviewViewExists(
            [Frozen] IOptions<GlobalSettings> globalSettings,
            PreviewController sut)
        {
            globalSettings.Value.SbnPath = "/";

            var viewResult = sut.Index() as ViewResult;
            var fileName = GetViewName(viewResult);

            IEnumerable<string> views = GetUiFiles(new[] { "sbn", "SbnBackOffice" });

            Assert.True(views.Contains(fileName), $"Expected {fileName} to exist, but it didn't");
        }

        [Test]
        [AutoMoqData]
        public async Task BackOfficeDefaultExists(
            [Frozen] IOptions<GlobalSettings> globalSettings,
            [Frozen] IHostingEnvironment hostingEnvironment,
            [Frozen] ITempDataDictionary tempDataDictionary,
            [Frozen] IRuntimeState runtimeState,
            BackOfficeController sut)
        {
            globalSettings.Value.SbnPath = "/";
            Mock.Get(hostingEnvironment).Setup(x => x.ToAbsolute("/")).Returns("http://localhost/");
            Mock.Get(hostingEnvironment).SetupGet(x => x.ApplicationVirtualPath).Returns("/");
            Mock.Get(runtimeState).Setup(x => x.Level).Returns(RuntimeLevel.Run);

            sut.TempData = tempDataDictionary;

            var viewResult = await sut.Default() as ViewResult;
            var fileName = GetViewName(viewResult);
            IEnumerable<string> views = GetUiFiles(new[] { "sbn", "SbnBackOffice" });

            Assert.True(views.Contains(fileName), $"Expected {fileName} to exist, but it didn't");
        }

        [Test]
        public void LanguageFilesAreLowercase()
        {
            IEnumerable<string> files = GetUiFiles(new[] { "sbn", "config", "lang" });
            foreach (var fileName in files)
            {
                Assert.AreEqual(
                    fileName.ToLower(),
                    fileName,
                    $"Language files must be all lowercase but {fileName} is not lowercase.");
            }
        }
    }
}
