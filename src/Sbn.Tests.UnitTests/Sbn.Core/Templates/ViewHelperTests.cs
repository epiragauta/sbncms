// Copyright (c) Sbn.
// See LICENSE for more details.

using NUnit.Framework;
using Sbn.Cms.Core.IO;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Core.Templates
{
    [TestFixture]
    public class ViewHelperTests
    {
        [Test]
        public void NoOptions()
        {
            var view = ViewHelper.GetDefaultFileContent();
            Assert.AreEqual(
                FixView(@"@using Sbn.Cms.Web.Common.PublishedModels;
@inherits Sbn.Cms.Web.Common.Views.SbnViewPage
@{
    Layout = null;
}"), FixView(view));
        }

        [Test]
        public void Layout()
        {
            var view = ViewHelper.GetDefaultFileContent(layoutPageAlias: "Dharznoik");
            Assert.AreEqual(
                FixView(@"@using Sbn.Cms.Web.Common.PublishedModels;
@inherits Sbn.Cms.Web.Common.Views.SbnViewPage
@{
    Layout = ""Dharznoik.cshtml"";
}"), FixView(view));
        }

        [Test]
        public void ClassName()
        {
            var view = ViewHelper.GetDefaultFileContent(modelClassName: "ClassName");
            Assert.AreEqual(
                FixView(@"@using Sbn.Cms.Web.Common.PublishedModels;
@inherits Sbn.Cms.Web.Common.Views.SbnViewPage<ClassName>
@{
    Layout = null;
}"), FixView(view));
        }

        [Test]
        public void Namespace()
        {
            var view = ViewHelper.GetDefaultFileContent(modelNamespace: "Models");
            Assert.AreEqual(
                FixView(@"@using Sbn.Cms.Web.Common.PublishedModels;
@inherits Sbn.Cms.Web.Common.Views.SbnViewPage
@{
    Layout = null;
}"), FixView(view));
        }

        [Test]
        public void ClassNameAndNamespace()
        {
            var view = ViewHelper.GetDefaultFileContent(modelClassName: "ClassName", modelNamespace: "My.Models");
            Assert.AreEqual(
                FixView(@"@using Sbn.Cms.Web.Common.PublishedModels;
@inherits Sbn.Cms.Web.Common.Views.SbnViewPage<ContentModels.ClassName>
@using ContentModels = My.Models;
@{
    Layout = null;
}"), FixView(view));
        }

        [Test]
        public void ClassNameAndNamespaceAndAlias()
        {
            var view = ViewHelper.GetDefaultFileContent(modelClassName: "ClassName", modelNamespace: "My.Models", modelNamespaceAlias: "MyModels");
            Assert.AreEqual(
                FixView(@"@using Sbn.Cms.Web.Common.PublishedModels;
@inherits Sbn.Cms.Web.Common.Views.SbnViewPage<MyModels.ClassName>
@using MyModels = My.Models;
@{
    Layout = null;
}"), FixView(view));
        }

        [Test]
        public void Combined()
        {
            var view = ViewHelper.GetDefaultFileContent(layoutPageAlias: "Dharznoik", modelClassName: "ClassName", modelNamespace: "My.Models", modelNamespaceAlias: "MyModels");
            Assert.AreEqual(
                FixView(@"@using Sbn.Cms.Web.Common.PublishedModels;
@inherits Sbn.Cms.Web.Common.Views.SbnViewPage<MyModels.ClassName>
@using MyModels = My.Models;
@{
    Layout = ""Dharznoik.cshtml"";
}"), FixView(view));
        }

        private static string FixView(string view)
        {
            view = view.Replace("\r\n", "\n");
            view = view.Replace("\r", "\n");
            view = view.Replace("\n", "\r\n");
            view = view.Replace("\t", "    ");
            return view;
        }
    }
}
