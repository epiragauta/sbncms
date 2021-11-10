// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Linq;
using System.Threading;
using NUnit.Framework;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Tests.Common.Testing;
using Sbn.Cms.Tests.Integration.Testing;

namespace Sbn.Cms.Tests.Integration.Sbn.Infrastructure.Services
{
    [TestFixture]
    [SbnTest(Database = SbnTestOptions.Database.NewSchemaPerTest)]
    public class FileServiceTests : SbnIntegrationTest
    {
        private IFileService FileService => GetRequiredService<IFileService>();

        [Test]
        public void Create_Template_Then_Assign_Child()
        {
            ITemplate child = FileService.CreateTemplateWithIdentity("Child", "child", "test");
            ITemplate parent = FileService.CreateTemplateWithIdentity("Parent", "parent", "test");

            child.SetMasterTemplate(parent);
            FileService.SaveTemplate(child);

            child = FileService.GetTemplate(child.Id);

            Assert.AreEqual(parent.Alias, child.MasterTemplateAlias);
        }

        [Test]
        public void Create_Template_With_Child_Then_Unassign()
        {
            ITemplate parent = FileService.CreateTemplateWithIdentity("Parent", "parent", "test");
            ITemplate child = FileService.CreateTemplateWithIdentity("Child", "child", "test", parent);

            child.SetMasterTemplate(null);
            FileService.SaveTemplate(child);

            child = FileService.GetTemplate(child.Id);

            Assert.AreEqual(null, child.MasterTemplateAlias);
        }

        [Test]
        public void Can_Query_Template_Children()
        {
            ITemplate parent = FileService.CreateTemplateWithIdentity("Parent", "parent", "test");
            ITemplate child1 = FileService.CreateTemplateWithIdentity("Child1", "child1", "test", parent);
            ITemplate child2 = FileService.CreateTemplateWithIdentity("Child2", "child2", "test", parent);

            int[] children = FileService.GetTemplates(parent.Id).Select(x => x.Id).ToArray();

            Assert.IsTrue(children.Contains(child1.Id));
            Assert.IsTrue(children.Contains(child2.Id));
        }

        [Test]
        public void Create_Template_With_Custom_Alias()
        {
            ITemplate template = FileService.CreateTemplateWithIdentity("Test template", "customTemplateAlias", "test");

            FileService.SaveTemplate(template);

            template = FileService.GetTemplate(template.Id);

            Assert.AreEqual("Test template", template.Name);
            Assert.AreEqual("customTemplateAlias", template.Alias);
        }
    }
}
