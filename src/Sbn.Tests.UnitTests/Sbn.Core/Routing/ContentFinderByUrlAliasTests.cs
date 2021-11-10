using System;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using Sbn.Cms.Core;
using Sbn.Cms.Core.Models.PublishedContent;
using Sbn.Cms.Core.PublishedCache;
using Sbn.Cms.Core.Routing;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Web;
using Sbn.Cms.Tests.UnitTests.AutoFixture;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Core.Routing
{
    [TestFixture]
    public class ContentFinderByUrlAliasTests
    {

        [Test]
        [InlineAutoMoqData("/this/is/my/alias", 1001)]
        [InlineAutoMoqData("/anotheralias", 1001)]
        [InlineAutoMoqData("/page2/alias", 10011)]
        [InlineAutoMoqData("/2ndpagealias", 10011)]
        [InlineAutoMoqData("/only/one/alias", 100111)]
        [InlineAutoMoqData("/ONLY/one/Alias", 100111)]
        [InlineAutoMoqData("/alias43", 100121)]
        public void Lookup_By_Url_Alias (
            string relativeUrl,
            int nodeMatch,
            [Frozen] IPublishedContentCache publishedContentCache,
            [Frozen] ISbnContextAccessor sbnContextAccessor,
            [Frozen] ISbnContext sbnContext,
            [Frozen] IVariationContextAccessor variationContextAccessor,
            IFileService fileService,
            ContentFinderByUrlAlias sut,
            IPublishedContent[] rootContents,
            IPublishedProperty urlProperty
            )
        {


            //Arrange
            var absoluteUrl = "http://localhost" + relativeUrl;
            VariationContext variationContext = new VariationContext();

            var contentItem = rootContents[0];
            Mock.Get(sbnContextAccessor).Setup(x => x.TryGetSbnContext(out sbnContext)).Returns(true);
            Mock.Get(sbnContext).Setup(x => x.Content).Returns(publishedContentCache);
            Mock.Get(publishedContentCache).Setup(x => x.GetAtRoot(null)).Returns(rootContents);
            Mock.Get(contentItem).Setup(x => x.Id).Returns(nodeMatch);
            Mock.Get(contentItem).Setup(x => x.GetProperty(Constants.Conventions.Content.UrlAlias)).Returns(urlProperty);
            Mock.Get(urlProperty).Setup(x => x.GetValue(null, null)).Returns(relativeUrl);

            Mock.Get(variationContextAccessor).Setup(x => x.VariationContext).Returns(variationContext);
            var publishedRequestBuilder = new PublishedRequestBuilder(new Uri(absoluteUrl, UriKind.Absolute), fileService);
            //Act
            var result = sut.TryFindContent(publishedRequestBuilder);

            Assert.IsTrue(result);
            Assert.AreEqual(publishedRequestBuilder.PublishedContent.Id, nodeMatch);
        }
    }
}
