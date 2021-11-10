using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Routing;
using Sbn.Cms.Tests.Common.Testing;
using Sbn.Tests.TestHelpers;

namespace Sbn.Tests.Routing
{
    [TestFixture]
    [SbnTest(Database = SbnTestOptions.Database.NewSchemaPerFixture)]
    public class ContentFinderByUrlAndTemplateTests : BaseWebTest
    {
        Template CreateTemplate(string alias)
        {
            var template = new Template(ShortStringHelper, alias, alias);
            template.Content = ""; // else saving throws with a dirty internal error
            ServiceContext.FileService.SaveTemplate(template);
            return template;
        }

        [TestCase("/blah")]
        [TestCase("/default.aspx/blah")] //this one is actually rather important since this is the path that comes through when we are running in pre-IIS 7 for the root document '/' !
        [TestCase("/home/Sub1/blah")]
        [TestCase("/Home/Sub1/Blah")] //different cases
        [TestCase("/home/Sub1.aspx/blah")]
        public async Task Match_Document_By_Url_With_Template(string urlAsString)
        {
            var globalSettings = new GlobalSettings { HideTopLevelNodeFromPath = false };

            var template1 = CreateTemplate("test");
            var template2 = CreateTemplate("blah");
            var sbnContext = GetSbnContext(urlAsString, template1.Id, globalSettings: globalSettings);
            var publishedRouter = CreatePublishedRouter(GetSbnContextAccessor(sbnContext));
            var reqBuilder = await publishedRouter.CreateRequestAsync(sbnContext.CleanedSbnUrl);
            var webRoutingSettings = new WebRoutingSettings();
            var lookup = new ContentFinderByUrlAndTemplate(
                LoggerFactory.CreateLogger<ContentFinderByUrlAndTemplate>(),
                ServiceContext.FileService,
                ServiceContext.ContentTypeService,
                GetSbnContextAccessor(sbnContext),
                Microsoft.Extensions.Options.Options.Create(webRoutingSettings));

            var result = lookup.TryFindContent(reqBuilder);

            IPublishedRequest frequest = reqBuilder.Build();

            Assert.IsTrue(result);
            Assert.IsNotNull(frequest.PublishedContent);
            var templateAlias = frequest.GetTemplateAlias();
            Assert.IsNotNull(templateAlias );
            Assert.AreEqual("blah".ToUpperInvariant(), templateAlias.ToUpperInvariant());
        }
    }
}
