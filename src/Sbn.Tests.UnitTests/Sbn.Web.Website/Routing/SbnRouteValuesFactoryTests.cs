using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Sbn.Cms.Core.Features;
using Sbn.Cms.Core.Models.PublishedContent;
using Sbn.Cms.Core.Routing;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Strings;
using Sbn.Cms.Web.Common.Controllers;
using Sbn.Cms.Web.Common.Routing;
using Sbn.Cms.Web.Website.Controllers;
using Sbn.Cms.Web.Website.Routing;
using Sbn.Extensions;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Web.Website.Routing
{

    [TestFixture]
    public class SbnRouteValuesFactoryTests
    {
        private SbnRouteValuesFactory GetFactory(
            out Mock<IPublishedRouter> publishedRouter,
            out IOptions<SbnRenderingDefaultsOptions> renderingDefaults,
            out IPublishedRequest request)
        {
            var builder = new PublishedRequestBuilder(new Uri("https://example.com"), Mock.Of<IFileService>());
            builder.SetPublishedContent(Mock.Of<IPublishedContent>());
            IPublishedRequest builtRequest = request = builder.Build();

            publishedRouter = new Mock<IPublishedRouter>();
            publishedRouter.Setup(x => x.UpdateRequestAsync(It.IsAny<IPublishedRequest>(), null))
                .Returns((IPublishedRequest r, IPublishedContent c) => Task.FromResult(builtRequest))
                .Verifiable();

            renderingDefaults = Mock.Of<IOptions<SbnRenderingDefaultsOptions>>(x => x.Value.DefaultControllerType == typeof(RenderController));

            // add the default one
            var actionDescriptors = new List<ActionDescriptor>
            {
                new ControllerActionDescriptor
                {
                    ControllerName = ControllerExtensions.GetControllerName<RenderController>(),
                    ActionName = nameof(RenderController.Index),
                    ControllerTypeInfo = typeof(RenderController).GetTypeInfo()
                }
            };
            var actionSelector = new Mock<IActionSelector>();
            actionSelector.Setup(x => x.SelectCandidates(It.IsAny<RouteContext>())).Returns(actionDescriptors);

            var factory = new SbnRouteValuesFactory(
                renderingDefaults,
                Mock.Of<IShortStringHelper>(),
                new SbnFeatures(),
                new ControllerActionSearcher(
                    new NullLogger<ControllerActionSearcher>(),
                    actionSelector.Object),
                publishedRouter.Object);

            return factory;
        }

        [Test]
        public async Task Update_Request_To_Not_Found_When_No_Template()
        {
            SbnRouteValuesFactory factory = GetFactory(out Mock<IPublishedRouter> publishedRouter, out _, out IPublishedRequest request);

            SbnRouteValues result = await factory.CreateAsync(new DefaultHttpContext(), request);

            // The request has content, no template, no hijacked route and no disabled template features so UpdateRequestToNotFound will be called
            publishedRouter.Verify(m => m.UpdateRequestAsync(It.IsAny<IPublishedRequest>(), null), Times.Once);
        }

        [Test]
        public async Task Adds_Result_To_Route_Value_Dictionary()
        {
            SbnRouteValuesFactory factory = GetFactory(out _, out IOptions<SbnRenderingDefaultsOptions> renderingDefaults, out IPublishedRequest request);

            SbnRouteValues result = await factory.CreateAsync(new DefaultHttpContext(), request);

            Assert.IsNotNull(result);
            Assert.AreEqual(renderingDefaults.Value.DefaultControllerType, result.ControllerType);
            Assert.AreEqual(SbnRouteValues.DefaultActionName, result.ActionName);
            Assert.IsNull(result.TemplateName);
        }
    }
}
