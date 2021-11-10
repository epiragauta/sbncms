using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Models.PublishedContent;
using Sbn.Cms.Core.Routing;
using Sbn.Tests.TestHelpers;

namespace Sbn.Tests.PublishedContent
{
    [TestFixture]
    public class PublishedRouterTests : BaseWebTest
    {
        [Test]
        public async Task ConfigureRequest_Returns_False_Without_HasPublishedContent()
        {
            var sbnContext = GetSbnContext("/test");
            var publishedRouter = CreatePublishedRouter(GetSbnContextAccessor(sbnContext));
            var request = await publishedRouter.CreateRequestAsync(sbnContext.CleanedSbnUrl);
            var result = publishedRouter.BuildRequest(request);

            Assert.IsFalse(result.Success());
        }

        [Test]
        public async Task ConfigureRequest_Returns_False_When_IsRedirect()
        {
            var sbnContext = GetSbnContext("/test");
            var publishedRouter = CreatePublishedRouter(GetSbnContextAccessor(sbnContext));
            var request = await publishedRouter.CreateRequestAsync(sbnContext.CleanedSbnUrl);
            var content = GetPublishedContentMock();
            request.SetPublishedContent(content.Object);
            request.SetCulture("en-AU");
            request.SetRedirect("/hello");
            var result = publishedRouter.BuildRequest(request);

            Assert.IsFalse(result.Success());
        }

        private Mock<IPublishedContent> GetPublishedContentMock()
        {
            var pc = new Mock<IPublishedContent>();
            pc.Setup(content => content.Id).Returns(1);
            pc.Setup(content => content.Name).Returns("test");
            pc.Setup(content => content.CreateDate).Returns(DateTime.Now);
            pc.Setup(content => content.UpdateDate).Returns(DateTime.Now);
            pc.Setup(content => content.Path).Returns("-1,1");
            pc.Setup(content => content.Parent).Returns(() => null);
            pc.Setup(content => content.Properties).Returns(new Collection<IPublishedProperty>());
            pc.Setup(content => content.ContentType).Returns(new PublishedContentType(Guid.NewGuid(), 22, "anything", PublishedItemType.Content, Enumerable.Empty<string>(), Enumerable.Empty<PublishedPropertyType>(), ContentVariation.Nothing));
            return pc;
        }
    }
}
