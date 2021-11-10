using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Sbn.Cms.Core;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Hosting;
using Sbn.Cms.Core.Routing;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Web;
using Sbn.Cms.Web.Common.Routing;
using Sbn.Cms.Web.Common.Security;
using Sbn.Cms.Web.Website.Controllers;
using Sbn.Extensions;
using static Sbn.Cms.Core.Constants.Web.Routing;
using RouteDirection = Sbn.Cms.Core.Routing.RouteDirection;

namespace Sbn.Cms.Web.Website.Routing
{

    /// <summary>
    /// The route value transformer for Sbn front-end routes
    /// </summary>
    /// <remarks>
    /// NOTE: In aspnet 5 DynamicRouteValueTransformer has been improved, see https://github.com/dotnet/aspnetcore/issues/21471
    /// It seems as though with the "State" parameter we could more easily assign the IPublishedRequest or IPublishedContent
    /// or SbnContext more easily that way. In the meantime we will rely on assigning the IPublishedRequest to the
    /// route values along with the IPublishedContent to the sbn context
    /// have created a GH discussion here https://github.com/dotnet/aspnetcore/discussions/28562 we'll see if anyone responds
    /// </remarks>
    public class SbnRouteValueTransformer : DynamicRouteValueTransformer
    {
        private readonly ILogger<SbnRouteValueTransformer> _logger;
        private readonly ISbnContextAccessor _sbnContextAccessor;
        private readonly IPublishedRouter _publishedRouter;
        private readonly GlobalSettings _globalSettings;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IRuntimeState _runtime;
        private readonly ISbnRouteValuesFactory _routeValuesFactory;
        private readonly IRoutableDocumentFilter _routableDocumentFilter;
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private readonly IControllerActionSearcher _controllerActionSearcher;
        private readonly IEventAggregator _eventAggregator;
        private readonly IPublicAccessRequestHandler _publicAccessRequestHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="SbnRouteValueTransformer"/> class.
        /// </summary>
        public SbnRouteValueTransformer(
            ILogger<SbnRouteValueTransformer> logger,
            ISbnContextAccessor sbnContextAccessor,
            IPublishedRouter publishedRouter,
            IOptions<GlobalSettings> globalSettings,
            IHostingEnvironment hostingEnvironment,
            IRuntimeState runtime,
            ISbnRouteValuesFactory routeValuesFactory,
            IRoutableDocumentFilter routableDocumentFilter,
            IDataProtectionProvider dataProtectionProvider,
            IControllerActionSearcher controllerActionSearcher,
            IEventAggregator eventAggregator,
            IPublicAccessRequestHandler publicAccessRequestHandler)
        {
            if (globalSettings is null)
            {
                throw new ArgumentNullException(nameof(globalSettings));
            }

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _sbnContextAccessor = sbnContextAccessor ?? throw new ArgumentNullException(nameof(sbnContextAccessor));
            _publishedRouter = publishedRouter ?? throw new ArgumentNullException(nameof(publishedRouter));
            _globalSettings = globalSettings.Value;
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
            _runtime = runtime ?? throw new ArgumentNullException(nameof(runtime));
            _routeValuesFactory = routeValuesFactory ?? throw new ArgumentNullException(nameof(routeValuesFactory));
            _routableDocumentFilter = routableDocumentFilter ?? throw new ArgumentNullException(nameof(routableDocumentFilter));
            _dataProtectionProvider = dataProtectionProvider;
            _controllerActionSearcher = controllerActionSearcher;
            _eventAggregator = eventAggregator;
            _publicAccessRequestHandler = publicAccessRequestHandler;
        }

        /// <inheritdoc/>
        public override async ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext, RouteValueDictionary values)
        {
            // If we aren't running, then we have nothing to route
            if (_runtime.Level != RuntimeLevel.Run)
            {
                return null;
            }
            // will be null for any client side requests like JS, etc...
            if (!_sbnContextAccessor.TryGetSbnContext(out ISbnContext sbnContext))
            {
                return null;
            }

            if (!_routableDocumentFilter.IsDocumentRequest(httpContext.Request.Path))
            {
                return null;
            }

            // Don't execute if there are already SbnRouteValues assigned.
            // This can occur if someone else is dynamically routing and in which case we don't want to overwrite
            // the routing work being done there.
            SbnRouteValues sbnRouteValues = httpContext.Features.Get<SbnRouteValues>();
            if (sbnRouteValues != null)
            {
                return null;
            }

            // Check if there is no existing content and return the no content controller
            if (!sbnContext.Content.HasContent())
            {
                return new RouteValueDictionary
                {
                    [ControllerToken] = ControllerExtensions.GetControllerName<RenderNoContentController>(),
                    [ActionToken] = nameof(RenderNoContentController.Index)
                };
            }

            IPublishedRequest publishedRequest = await RouteRequestAsync(sbnContext);

            sbnRouteValues = await _routeValuesFactory.CreateAsync(httpContext, publishedRequest);            

            // now we need to do some public access checks
            sbnRouteValues = await _publicAccessRequestHandler.RewriteForPublishedContentAccessAsync(httpContext, sbnRouteValues);

            // Store the route values as a httpcontext feature
            httpContext.Features.Set(sbnRouteValues);

            // Need to check if there is form data being posted back to an Sbn URL
            PostedDataProxyInfo postedInfo = GetFormInfo(httpContext, values);
            if (postedInfo != null)
            {
                return HandlePostedValues(postedInfo, httpContext);
            }

            SbnRouteResult? routeResult = sbnRouteValues?.PublishedRequest?.GetRouteResult();

            if (!routeResult.HasValue || routeResult == SbnRouteResult.NotFound)
            {
                // No content was found, not by any registered 404 handlers and
                // not by the IContentLastChanceFinder. In this case we want to return
                // our default 404 page but we cannot return route values now because
                // it's possible that a developer is handling dynamic routes too.
                // Our 404 page will be handled with the NotFoundSelectorPolicy
                return null;
            }

            // See https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.routing.dynamicroutevaluetransformer.transformasync?view=aspnetcore-5.0#Microsoft_AspNetCore_Mvc_Routing_DynamicRouteValueTransformer_TransformAsync_Microsoft_AspNetCore_Http_HttpContext_Microsoft_AspNetCore_Routing_RouteValueDictionary_
            // We should apparenlty not be modified these values.
            // So we create new ones.
            var newValues = new RouteValueDictionary
            {
                [ControllerToken] = sbnRouteValues.ControllerName
            };
            if (string.IsNullOrWhiteSpace(sbnRouteValues.ActionName) == false)
            {
                newValues[ActionToken] = sbnRouteValues.ActionName;
            }

            return newValues;
        }

        private async Task<IPublishedRequest> RouteRequestAsync(ISbnContext sbnContext)
        {
            // ok, process

            // instantiate, prepare and process the published content request
            // important to use CleanedSbnUrl - lowercase path-only version of the current url
            IPublishedRequestBuilder requestBuilder = await _publishedRouter.CreateRequestAsync(sbnContext.CleanedSbnUrl);

            // TODO: This is ugly with the re-assignment to sbn context but at least its now
            // an immutable object. The only way to make this better would be to have a RouteRequest
            // as part of SbnContext but then it will require a PublishedRouter dependency so not sure that's worth it.
            // Maybe could be a one-time Set method instead?
            IPublishedRequest routedRequest = await _publishedRouter.RouteRequestAsync(requestBuilder, new RouteRequestOptions(RouteDirection.Inbound));
            sbnContext.PublishedRequest = routedRequest;

            return routedRequest;
        }

        /// <summary>
        /// Checks the request and query strings to see if it matches the definition of having a Surface controller
        /// posted/get value, if so, then we return a PostedDataProxyInfo object with the correct information.
        /// </summary>
        private PostedDataProxyInfo GetFormInfo(HttpContext httpContext, RouteValueDictionary values)
        {
            if (httpContext is null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            // if it is a POST/GET then a value must be in the request
            if (!httpContext.Request.Query.TryGetValue("ufprt", out StringValues encodedVal)
                && (!httpContext.Request.HasFormContentType || !httpContext.Request.Form.TryGetValue("ufprt", out encodedVal)))
            {
                return null;
            }

            if (!EncryptionHelper.DecryptAndValidateEncryptedRouteString(
                _dataProtectionProvider,
                encodedVal,
                out IDictionary<string, string> decodedParts))
            {
                return null;
            }

            // Get all route values that are not the default ones and add them separately so they eventually get to action parameters
            foreach (KeyValuePair<string, string> item in decodedParts.Where(x => ReservedAdditionalKeys.AllKeys.Contains(x.Key) == false))
            {
                values[item.Key] = item.Value;
            }

            // return the proxy info without the surface id... could be a local controller.
            return new PostedDataProxyInfo
            {
                ControllerName = WebUtility.UrlDecode(decodedParts.First(x => x.Key == ReservedAdditionalKeys.Controller).Value),
                ActionName = WebUtility.UrlDecode(decodedParts.First(x => x.Key == ReservedAdditionalKeys.Action).Value),
                Area = WebUtility.UrlDecode(decodedParts.First(x => x.Key == ReservedAdditionalKeys.Area).Value),
            };
        }

        private RouteValueDictionary HandlePostedValues(PostedDataProxyInfo postedInfo, HttpContext httpContext)
        {
            // set the standard route values/tokens
            var values = new RouteValueDictionary
            {
                [ControllerToken] = postedInfo.ControllerName,
                [ActionToken] = postedInfo.ActionName
            };

            ControllerActionDescriptor surfaceControllerDescriptor = _controllerActionSearcher.Find<SurfaceController>(httpContext, postedInfo.ControllerName, postedInfo.ActionName);

            if (surfaceControllerDescriptor == null)
            {
                throw new InvalidOperationException("Could not find a Surface controller route in the RouteTable for controller name " + postedInfo.ControllerName);
            }

            // set the area if one is there.
            if (!postedInfo.Area.IsNullOrWhiteSpace())
            {
                values["area"] = postedInfo.Area;
            }

            return values;
        }

        private class PostedDataProxyInfo
        {
            public string ControllerName { get; set; }

            public string ActionName { get; set; }

            public string Area { get; set; }
        }

        // Define reserved dictionary keys for controller, action and area specified in route additional values data
        private static class ReservedAdditionalKeys
        {
            internal static readonly string[] AllKeys = new[]
            {
                Controller,
                Action,
                Area
            };

            internal const string Controller = "c";
            internal const string Action = "a";
            internal const string Area = "ar";
        }
    }
}
