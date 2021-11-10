using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Sbn.Cms.Core;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models.PublishedContent;
using Sbn.Cms.Core.PublishedCache;
using Sbn.Cms.Core.Routing;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Web;
using Sbn.Cms.Tests.UnitTests.TestHelpers;
using Sbn.Cms.Web.Common.Controllers;
using Sbn.Cms.Web.Common.Routing;
using Sbn.Cms.Web.Website.Controllers;
using Sbn.Cms.Web.Website.Routing;
using Sbn.Extensions;
using static Sbn.Cms.Core.Constants.Web.Routing;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Web.Website.Routing
{
    [TestFixture]
    public class SbnRouteValueTransformerTests
    {
        private IOptions<GlobalSettings> GetGlobalSettings() => Options.Create(new GlobalSettings());

        private SbnRouteValueTransformer GetTransformerWithRunState(
            ISbnContextAccessor ctx,
            IRoutableDocumentFilter filter = null,
            IPublishedRouter router = null,
            ISbnRouteValuesFactory routeValuesFactory = null)
            => GetTransformer(ctx, Mock.Of<IRuntimeState>(x => x.Level == RuntimeLevel.Run), filter, router, routeValuesFactory);

        private SbnRouteValueTransformer GetTransformer(
            ISbnContextAccessor ctx,
            IRuntimeState state,
            IRoutableDocumentFilter filter = null,
            IPublishedRouter router = null,
            ISbnRouteValuesFactory routeValuesFactory = null)
        {
            var publicAccessRequestHandler = new Mock<IPublicAccessRequestHandler>();
            publicAccessRequestHandler.Setup(x => x.RewriteForPublishedContentAccessAsync(It.IsAny<HttpContext>(), It.IsAny<SbnRouteValues>()))
                .Returns((HttpContext ctx, SbnRouteValues routeVals) => Task.FromResult(routeVals));

            var transformer = new SbnRouteValueTransformer(
                new NullLogger<SbnRouteValueTransformer>(),
                ctx,
                router ?? Mock.Of<IPublishedRouter>(),
                GetGlobalSettings(),
                TestHelper.GetHostingEnvironment(),
                state,
                routeValuesFactory ?? Mock.Of<ISbnRouteValuesFactory>(),
                filter ?? Mock.Of<IRoutableDocumentFilter>(x => x.IsDocumentRequest(It.IsAny<string>()) == true),
                Mock.Of<IDataProtectionProvider>(),
                Mock.Of<IControllerActionSearcher>(),
                Mock.Of<IEventAggregator>(),
                publicAccessRequestHandler.Object);

            return transformer;
        }

        private ISbnContext GetSbnContext(bool hasContent)
        {
            IPublishedContentCache publishedContent = Mock.Of<IPublishedContentCache>(x => x.HasContent() == hasContent);
            var uri = new Uri("http://example.com");

            ISbnContext sbnContext = Mock.Of<ISbnContext>(x =>
                x.Content == publishedContent
                && x.OriginalRequestUrl == uri
                && x.CleanedSbnUrl == uri);

            return sbnContext;
        }

        private SbnRouteValues GetRouteValues(IPublishedRequest request)
            => new SbnRouteValues(
                request,
                new ControllerActionDescriptor
                {
                    ControllerTypeInfo = typeof(TestController).GetTypeInfo(),
                    ControllerName = ControllerExtensions.GetControllerName<TestController>()
                });

        private ISbnRouteValuesFactory GetRouteValuesFactory(IPublishedRequest request)
            => Mock.Of<ISbnRouteValuesFactory>(x => x.CreateAsync(It.IsAny<HttpContext>(), It.IsAny<IPublishedRequest>()) == Task.FromResult(GetRouteValues(request)));

        private IPublishedRouter GetRouter(IPublishedRequest request)
            => Mock.Of<IPublishedRouter>(x => x.RouteRequestAsync(It.IsAny<IPublishedRequestBuilder>(), It.IsAny<RouteRequestOptions>()) == Task.FromResult(request));

        [Test]
        public async Task Null_When_Runtime_Level_Not_Run()
        {
            SbnRouteValueTransformer transformer = GetTransformer(
                Mock.Of<ISbnContextAccessor>(),
                Mock.Of<IRuntimeState>());

            RouteValueDictionary result = await transformer.TransformAsync(new DefaultHttpContext(), new RouteValueDictionary());
            Assert.IsNull(result);
        }

        [Test]
        public async Task Null_When_No_Sbn_Context()
        {
            SbnRouteValueTransformer transformer = GetTransformerWithRunState(
                Mock.Of<ISbnContextAccessor>());

            RouteValueDictionary result = await transformer.TransformAsync(new DefaultHttpContext(), new RouteValueDictionary());
            Assert.IsNull(result);
        }

        [Test]
        public async Task Null_When_Not_Document_Request()
        {
            var sbnContext = Mock.Of<ISbnContext>();
            SbnRouteValueTransformer transformer = GetTransformerWithRunState(
                Mock.Of<ISbnContextAccessor>(x => x.TryGetSbnContext(out sbnContext)),
                Mock.Of<IRoutableDocumentFilter>(x => x.IsDocumentRequest(It.IsAny<string>()) == false));

            RouteValueDictionary result = await transformer.TransformAsync(new DefaultHttpContext(), new RouteValueDictionary());
            Assert.IsNull(result);
        }

        [Test]
        public async Task NoContentController_Values_When_No_Content()
        {
            ISbnContext sbnContext = GetSbnContext(false);

            SbnRouteValueTransformer transformer = GetTransformerWithRunState(
                Mock.Of<ISbnContextAccessor>(x => x.TryGetSbnContext(out sbnContext)));

            RouteValueDictionary result = await transformer.TransformAsync(new DefaultHttpContext(), new RouteValueDictionary());
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(ControllerExtensions.GetControllerName<RenderNoContentController>(), result[ControllerToken]);
            Assert.AreEqual(nameof(RenderNoContentController.Index), result[ActionToken]);
        }

        [Test]
        public async Task Assigns_PublishedRequest_To_SbnContext()
        {
            ISbnContext sbnContext = GetSbnContext(true);
            IPublishedRequest request = Mock.Of<IPublishedRequest>();

            SbnRouteValueTransformer transformer = GetTransformerWithRunState(
                Mock.Of<ISbnContextAccessor>(x => x.TryGetSbnContext(out sbnContext)),
                router: GetRouter(request),
                routeValuesFactory: GetRouteValuesFactory(request));

            RouteValueDictionary result = await transformer.TransformAsync(new DefaultHttpContext(), new RouteValueDictionary());
            Assert.AreEqual(request, sbnContext.PublishedRequest);
        }

        [Test]
        public async Task Null_When_No_Content_On_PublishedRequest()
        {
            ISbnContext sbnContext = GetSbnContext(true);
            IPublishedRequest request = Mock.Of<IPublishedRequest>(x => x.PublishedContent == null);

            SbnRouteValueTransformer transformer = GetTransformerWithRunState(
                Mock.Of<ISbnContextAccessor>(x => x.TryGetSbnContext(out sbnContext)),
                router: GetRouter(request),
                routeValuesFactory: GetRouteValuesFactory(request));

            var httpContext = new DefaultHttpContext();
            RouteValueDictionary result = await transformer.TransformAsync(httpContext, new RouteValueDictionary());
            Assert.IsNull(result);

            SbnRouteValues routeVals = httpContext.Features.Get<SbnRouteValues>();
            Assert.AreEqual(routeVals.PublishedRequest.GetRouteResult(), SbnRouteResult.NotFound);
        }

        [Test]
        public async Task Assigns_SbnRouteValues_To_HttpContext_Feature()
        {
            ISbnContext sbnContext = GetSbnContext(true);
            IPublishedRequest request = Mock.Of<IPublishedRequest>(x => x.PublishedContent == Mock.Of<IPublishedContent>());

            SbnRouteValueTransformer transformer = GetTransformerWithRunState(
                Mock.Of<ISbnContextAccessor>(x => x.TryGetSbnContext(out sbnContext)),
                router: GetRouter(request),
                routeValuesFactory: GetRouteValuesFactory(request));

            var httpContext = new DefaultHttpContext();
            RouteValueDictionary result = await transformer.TransformAsync(httpContext, new RouteValueDictionary());

            SbnRouteValues routeVals = httpContext.Features.Get<SbnRouteValues>();
            Assert.IsNotNull(routeVals);
            Assert.AreEqual(routeVals.PublishedRequest, sbnContext.PublishedRequest);
        }

        [Test]
        public async Task Assigns_Values_To_RouteValueDictionary_When_Content()
        {
            ISbnContext sbnContext = GetSbnContext(true);
            IPublishedRequest request = Mock.Of<IPublishedRequest>(x => x.PublishedContent == Mock.Of<IPublishedContent>());
            SbnRouteValues routeValues = GetRouteValues(request);

            SbnRouteValueTransformer transformer = GetTransformerWithRunState(
                Mock.Of<ISbnContextAccessor>(x => x.TryGetSbnContext(out sbnContext)),
                router: GetRouter(request),
                routeValuesFactory: GetRouteValuesFactory(request));

            RouteValueDictionary result = await transformer.TransformAsync(new DefaultHttpContext(), new RouteValueDictionary());

            Assert.AreEqual(routeValues.ControllerName, result[ControllerToken]);
            Assert.AreEqual(routeValues.ActionName, result[ActionToken]);
        }

        [Test]
        public async Task Returns_Null_RouteValueDictionary_When_No_Content()
        {
            ISbnContext sbnContext = GetSbnContext(true);
            IPublishedRequest request = Mock.Of<IPublishedRequest>(x => x.PublishedContent == null);
            SbnRouteValues routeValues = GetRouteValues(request);

            SbnRouteValueTransformer transformer = GetTransformerWithRunState(
                Mock.Of<ISbnContextAccessor>(x => x.TryGetSbnContext(out sbnContext)),
                router: GetRouter(request),
                routeValuesFactory: GetRouteValuesFactory(request));

            RouteValueDictionary result = await transformer.TransformAsync(new DefaultHttpContext(), new RouteValueDictionary());

            Assert.IsNull(result);
        }

        private class TestController : RenderController
        {
            public TestController(ILogger<TestController> logger, ICompositeViewEngine compositeViewEngine, ISbnContextAccessor sbnContextAccessor)
                : base(logger, compositeViewEngine, sbnContextAccessor)
            {
            }
        }
    }
}
