using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Sbn.Cms.Core.Logging;
using Sbn.Cms.Web.Common.Routing;
using Sbn.Cms.Web.Website.Controllers;
using static Sbn.Cms.Core.Constants.Web.Routing;

namespace Sbn.Cms.Web.Website.ActionResults
{
    /// <summary>
    /// Used by posted forms to proxy the result to the page in which the current URL matches on
    /// </summary>
    /// <remarks>
    /// This page does not redirect therefore it does not implement <see cref="IKeepTempDataResult"/> because TempData should
    /// only be used in situations when a redirect occurs. It is not good practice to use TempData when redirects do not occur
    /// so we'll be strict about it and not save it.
    /// </remarks>
    public class SbnPageResult : IActionResult
    {
        private readonly IProfilingLogger _profilingLogger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SbnPageResult"/> class.
        /// </summary>
        public SbnPageResult(IProfilingLogger profilingLogger) => _profilingLogger = profilingLogger;

        /// <inheritdoc/>
        public async Task ExecuteResultAsync(ActionContext context)
        {
            SbnRouteValues sbnRouteValues = context.HttpContext.Features.Get<SbnRouteValues>();
            if (sbnRouteValues == null)
            {
                throw new InvalidOperationException($"Can only use {nameof(SbnPageResult)} in the context of an Http POST when using a {nameof(SurfaceController)} form");
            }

            // Change the route values back to the original request vals
            context.RouteData.Values[ControllerToken] = sbnRouteValues.ControllerName;
            context.RouteData.Values[ActionToken] = sbnRouteValues.ActionName;

            // Create a new context and excute the original controller...
            // Copy the action context - this also copies the ModelState
            var renderActionContext = new ActionContext(context)
            {
                ActionDescriptor = sbnRouteValues.ControllerActionDescriptor
            };
            IActionInvokerFactory actionInvokerFactory = context.HttpContext.RequestServices.GetRequiredService<IActionInvokerFactory>();
            IActionInvoker actionInvoker = actionInvokerFactory.CreateInvoker(renderActionContext);
            await ExecuteControllerAction(actionInvoker);
        }

        /// <summary>
        /// Executes the controller action
        /// </summary>
        private async Task ExecuteControllerAction(IActionInvoker actionInvoker)
        {
            using (_profilingLogger.TraceDuration<SbnPageResult>("Executing Sbn RouteDefinition controller", "Finished"))
            {
                await actionInvoker.InvokeAsync();
            }
        }
    }
}
