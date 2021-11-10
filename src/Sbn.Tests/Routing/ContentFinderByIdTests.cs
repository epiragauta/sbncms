using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.Routing;
using Sbn.Cms.Core.Web;
using Sbn.Tests.TestHelpers;

namespace Sbn.Tests.Routing
{
    [TestFixture]
    public class ContentFinderByIdTests : BaseWebTest
    {
        [TestCase("/1046", 1046)]
        [TestCase("/1046.aspx", 1046)]
        public async Task Lookup_By_Id(string urlAsString, int nodeMatch)
        {
            var sbnContext = GetSbnContext(urlAsString);
            var publishedRouter = CreatePublishedRouter(GetSbnContextAccessor(sbnContext));
            var frequest = await publishedRouter.CreateRequestAsync(sbnContext.CleanedSbnUrl);
            var webRoutingSettings = new WebRoutingSettings();
            var lookup = new ContentFinderByIdPath(Microsoft.Extensions.Options.Options.Create(webRoutingSettings), LoggerFactory.CreateLogger<ContentFinderByIdPath>(), Factory.GetRequiredService<IRequestAccessor>(), GetSbnContextAccessor(sbnContext));


            var result = lookup.TryFindContent(frequest);

            Assert.IsTrue(result);
            Assert.AreEqual(frequest.PublishedContent.Id, nodeMatch);
        }
    }
}
