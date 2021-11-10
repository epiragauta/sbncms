using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using Sbn.Cms.Core.Models.PublishedContent;
using Sbn.Cms.Core.Routing;
using Sbn.Cms.Web.Common.Routing;

namespace Sbn.Cms.Web.Common.Controllers
{
    /// <summary>
    /// An abstract controller for a front-end Sbn page
    /// </summary>
    public abstract class SbnPageController : SbnController
    {
        private SbnRouteValues _sbnRouteValues;
        private readonly ICompositeViewEngine _compositeViewEngine;
        private readonly ILogger<SbnPageController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SbnPageController"/> class.
        /// </summary>
        protected SbnPageController(ILogger<SbnPageController> logger, ICompositeViewEngine compositeViewEngine)
        {
            _logger = logger;
            _compositeViewEngine = compositeViewEngine;
        }

        /// <summary>
        /// Gets the <see cref="SbnRouteValues"/>
        /// </summary>
        protected virtual SbnRouteValues SbnRouteValues
        {
            get
            {
                if (_sbnRouteValues != null)
                {
                    return _sbnRouteValues;
                }

                _sbnRouteValues = HttpContext.Features.Get<SbnRouteValues>();

                if (_sbnRouteValues == null)
                {
                    throw new InvalidOperationException($"No {nameof(SbnRouteValues)} feature was found in the HttpContext");
                }

                return _sbnRouteValues;
            }
        }

        /// <summary>
        /// Gets the current content item.
        /// </summary>
        protected virtual IPublishedContent CurrentPage
        {
            get
            {
                if (!SbnRouteValues.PublishedRequest.HasPublishedContent())
                {
                    // This will never be accessed this way since the controller will handle redirects and not founds
                    // before this can be accessed but we need to be explicit.
                    throw new InvalidOperationException("There is no published content found in the request");
                }

                return SbnRouteValues.PublishedRequest.PublishedContent;
            }
        }

        /// <summary>
        /// Gets an action result based on the template name found in the route values and a model.
        /// </summary>
        /// <typeparam name="T">The type of the model.</typeparam>
        /// <param name="model">The model.</param>
        /// <returns>The action result.</returns>
        /// <exception cref="InvalidOperationException">If the template found in the route values doesn't physically exist and exception is thrown</exception>
        protected IActionResult CurrentTemplate<T>(T model)
        {
            if (EnsurePhsyicalViewExists(SbnRouteValues.TemplateName) == false)
            {
                throw new InvalidOperationException("No physical template file was found for template " + SbnRouteValues.TemplateName);
            }

            return View(SbnRouteValues.TemplateName, model);
        }

        /// <summary>
        /// Ensures that a physical view file exists on disk.
        /// </summary>
        /// <param name="template">The view name.</param>
        protected bool EnsurePhsyicalViewExists(string template)
        {
            ViewEngineResult result = _compositeViewEngine.FindView(ControllerContext, template, false);
            if (result.View != null)
            {
                return true;
            }

            _logger.LogWarning("No physical template file was found for template {Template}", template);
            return false;
        }

    }
}
