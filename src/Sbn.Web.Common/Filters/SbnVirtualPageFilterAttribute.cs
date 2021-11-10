using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Sbn.Cms.Core.Models.PublishedContent;
using Sbn.Cms.Core.Routing;
using Sbn.Cms.Core.Web;
using Sbn.Cms.Web.Common.Controllers;
using Sbn.Cms.Web.Common.Routing;
using Sbn.Extensions;

namespace Sbn.Cms.Web.Common.Filters
{
    /// <summary>
    /// Used to set the <see cref="SbnRouteValues"/> request feature based on the <see cref="CustomRouteContentFinderDelegate"/> specified (if any)
    /// for the custom route.
    /// </summary>
    public class SbnVirtualPageFilterAttribute : Attribute, IAsyncActionFilter
    {
        /// <inheritdoc/>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Endpoint endpoint = context.HttpContext.GetEndpoint();

            // Check if there is any delegate in the metadata of the route, this
            // will occur when using the ForSbn method during routing.
            CustomRouteContentFinderDelegate contentFinder = endpoint.Metadata.OfType<CustomRouteContentFinderDelegate>().FirstOrDefault();

            if (contentFinder != null)
            {
                await SetSbnRouteValues(context, contentFinder.FindContent(context));
            }
            else
            {
                // Check if the controller is IVirtualPageController and then use that to FindContent
                if (context.Controller is IVirtualPageController ctrl)
                {
                    await SetSbnRouteValues(context, ctrl.FindContent(context));
                }
            }

            // if we've assigned not found, just exit
            if (!(context.Result is NotFoundResult))
            {
                await next();
            }
        }

        private async Task SetSbnRouteValues(ActionExecutingContext context, IPublishedContent content)
        {
            if (content != null)
            {
                ISbnContextAccessor sbnContextAccessor = context.HttpContext.RequestServices.GetRequiredService<ISbnContextAccessor>();
                IPublishedRouter router = context.HttpContext.RequestServices.GetRequiredService<IPublishedRouter>();

                var sbnContext = sbnContextAccessor.GetRequiredSbnContext();

                IPublishedRequestBuilder requestBuilder = await router.CreateRequestAsync(sbnContext.CleanedSbnUrl);
                requestBuilder.SetPublishedContent(content);
                IPublishedRequest publishedRequest = requestBuilder.Build();

                var routeValues = new SbnRouteValues(
                    publishedRequest,
                    (ControllerActionDescriptor)context.ActionDescriptor);

                context.HttpContext.Features.Set(routeValues);
            }
            else
            {
                // if there is no content then it should be a not found
                context.Result = new NotFoundResult();
            }
        }
    }
}
