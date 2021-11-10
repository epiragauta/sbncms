using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Sbn.Cms.Core.Routing;
using Sbn.Cms.Core.Web;
using Sbn.Tests.TestHelpers;

namespace Sbn.Tests.Routing
{
    [TestFixture]
    public class ContentFinderByPageIdQueryTests : BaseWebTest
    {
        [TestCase("/?umbPageId=1046", 1046)]
        [TestCase("/?UMBPAGEID=1046", 1046)]
        [TestCase("/default.aspx?umbPageId=1046", 1046)] // TODO: Should this match??
        [TestCase("/some/other/page?umbPageId=1046", 1046)] // TODO: Should this match??
        [TestCase("/some/other/page.aspx?umbPageId=1046", 1046)] // TODO: Should this match??
        public async Task Lookup_By_Page_Id(string urlAsString, int nodeMatch)
        {
            var sbnContext = GetSbnContext(urlAsString);
            var httpContext = GetHttpContextFactory(urlAsString).HttpContext;
            var publishedRouter = CreatePublishedRouter(GetSbnContextAccessor(sbnContext));
            var frequest = await publishedRouter .CreateRequestAsync(sbnContext.CleanedSbnUrl);
            var mockRequestAccessor = new Mock<IRequestAccessor>();
            mockRequestAccessor.Setup(x => x.GetRequestValue("umbPageID")).Returns(httpContext.Request.QueryString["umbPageID"]);

            var lookup = new ContentFinderByPageIdQuery(mockRequestAccessor.Object, GetSbnContextAccessor(sbnContext));

            var result = lookup.TryFindContent(frequest);

            Assert.IsTrue(result);
            Assert.AreEqual(frequest.PublishedContent.Id, nodeMatch);
        }
    }
}
