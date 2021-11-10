using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Options;
using Sbn.Cms.Core.Features;
using Sbn.Cms.Core.Models.PublishedContent;
using Sbn.Cms.Core.Routing;
using Sbn.Cms.Core.Strings;
using Sbn.Cms.Web.Common.Controllers;
using Sbn.Cms.Web.Common.Routing;
using Sbn.Cms.Web.Website.Controllers;
using Sbn.Extensions;

namespace Sbn.Cms.Web.Website.Routing
{
    /// <summary>
    /// Used to create <see cref="SbnRouteValues"/>
    /// </summary>
    public class SbnRouteValuesFactory : ISbnRouteValuesFactory
    {
        private readonly IShortStringHelper _shortStringHelper;
        private readonly SbnFeatures _sbnFeatures;
        private readonly IControllerActionSearcher _controllerActionSearcher;
        private readonly IPublishedRouter _publishedRouter;
        private readonly Lazy<string> _defaultControllerName;
        private readonly Lazy<ControllerActionDescriptor> _defaultControllerDescriptor;

        /// <summary>
        /// Initializes a new instance of the <see cref="SbnRouteValuesFactory"/> class.
        /// </summary>
        public SbnRouteValuesFactory(
            IOptions<SbnRenderingDefaultsOptions> renderingDefaults,
            IShortStringHelper shortStringHelper,
            SbnFeatures sbnFeatures,
            IControllerActionSearcher controllerActionSearcher,
            IPublishedRouter publishedRouter)
        {
            _shortStringHelper = shortStringHelper;
            _sbnFeatures = sbnFeatures;
            _controllerActionSearcher = controllerActionSearcher;
            _publishedRouter = publishedRouter;
            _defaultControllerName = new Lazy<string>(() => ControllerExtensions.GetControllerName(renderingDefaults.Value.DefaultControllerType));
            _defaultControllerDescriptor = new Lazy<ControllerActionDescriptor>(() =>
            {
                ControllerActionDescriptor descriptor = _controllerActionSearcher.Find<IRenderController>(
                    new DefaultHttpContext(), // this actually makes no difference for this method
                    DefaultControllerName,
                    SbnRouteValues.DefaultActionName);

                if (descriptor == null)
                {
                    throw new InvalidOperationException($"No controller/action found by name {DefaultControllerName}.{SbnRouteValues.DefaultActionName}");
                }

                return descriptor;
            });
        }

        /// <summary>
        /// Gets the default controller name
        /// </summary>
        protected string DefaultControllerName => _defaultControllerName.Value;

        /// <inheritdoc/>
        public async Task<SbnRouteValues> CreateAsync(HttpContext httpContext, IPublishedRequest request)
        {
            if (httpContext is null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            string customActionName = null;

            // check that a template is defined), if it doesn't and there is a hijacked route it will just route
            // to the index Action
            if (request.HasTemplate())
            {
                // the template Alias should always be already saved with a safe name.
                // if there are hyphens in the name and there is a hijacked route, then the Action will need to be attributed
                // with the action name attribute.
                customActionName = request.GetTemplateAlias()?.Split('.')[0].ToSafeAlias(_shortStringHelper);
            }

            // The default values for the default controller/action
            var def = new SbnRouteValues(
                request,
                _defaultControllerDescriptor.Value,
                templateName: customActionName);

            def = CheckHijackedRoute(httpContext, def, out bool hasHijackedRoute);

            def = await CheckNoTemplateAsync(httpContext, def, hasHijackedRoute);

            return def;
        }

        /// <summary>
        /// Check if the route is hijacked and return new route values
        /// </summary>
        private SbnRouteValues CheckHijackedRoute(HttpContext httpContext, SbnRouteValues def, out bool hasHijackedRoute)
        {
            IPublishedRequest request = def.PublishedRequest;

            var customControllerName = request.PublishedContent?.ContentType?.Alias;
            if (customControllerName != null)
            {
                ControllerActionDescriptor descriptor = _controllerActionSearcher.Find<IRenderController>(httpContext, customControllerName, def.TemplateName);
                if (descriptor != null)
                {
                    hasHijackedRoute = true;

                    return new SbnRouteValues(
                        request,
                        descriptor,
                        def.TemplateName);
                }
            }

            hasHijackedRoute = false;
            return def;
        }

        /// <summary>
        /// Special check for when no template or hijacked route is done which needs to re-run through the routing pipeline again for last chance finders
        /// </summary>
        private async Task<SbnRouteValues> CheckNoTemplateAsync(HttpContext httpContext, SbnRouteValues def, bool hasHijackedRoute)
        {
            IPublishedRequest request = def.PublishedRequest;

            // Here we need to check if there is no hijacked route and no template assigned but there is a content item.
            // If this is the case we want to return a blank page.
            // We also check if templates have been disabled since if they are then we're allowed to render even though there's no template,
            // for example for json rendering in headless.
            if (request.HasPublishedContent()
                && !request.HasTemplate()
                && !_sbnFeatures.Disabled.DisableTemplates
                && !hasHijackedRoute)
            {
                IPublishedContent content = request.PublishedContent;

                // This is basically a 404 even if there is content found.
                // We then need to re-run this through the pipeline for the last
                // chance finders to work.
                // Set to null since we are telling it there is no content.
                request = await _publishedRouter.UpdateRequestAsync(request, null);

                if (request == null)
                {
                    throw new InvalidOperationException($"The call to {nameof(IPublishedRouter.UpdateRequestAsync)} cannot return null");
                }

                def = new SbnRouteValues(
                        request,
                        def.ControllerActionDescriptor,
                        def.TemplateName);

                // if the content has changed, we must then again check for hijacked routes
                if (content != request.PublishedContent)
                {
                    def = CheckHijackedRoute(httpContext, def, out _);
                }
            }

            return def;
        }
    }
}
