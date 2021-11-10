using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Controllers;
using Sbn.Cms.Core.Features;

namespace Sbn.Cms.Web.Common.Authorization
{
    /// <summary>
    /// Ensures that the controller is an authorized feature.
    /// </summary>
    public class FeatureAuthorizeHandler : AuthorizationHandler<FeatureAuthorizeRequirement>
    {
        private readonly SbnFeatures _sbnFeatures;

        public FeatureAuthorizeHandler(SbnFeatures sbnFeatures)
        {
            _sbnFeatures = sbnFeatures;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, FeatureAuthorizeRequirement requirement)
        {
            var allowed = IsAllowed(context);
            if (!allowed.HasValue || allowed.Value)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
            return Task.CompletedTask;
        }

        private bool? IsAllowed(AuthorizationHandlerContext context)
        {
            Endpoint? endpoint = null;

            switch (context.Resource)
            {
                case DefaultHttpContext defaultHttpContext:
                {
                    IEndpointFeature endpointFeature = defaultHttpContext.Features.Get<IEndpointFeature>();
                    endpoint = endpointFeature.Endpoint;
                    break;
                }

                case Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext authorizationFilterContext:
                {
                    IEndpointFeature endpointFeature = authorizationFilterContext.HttpContext.Features.Get<IEndpointFeature>();
                    endpoint = endpointFeature.Endpoint;
                    break;
                }

                case Endpoint resourceEndpoint:
                {
                    endpoint = resourceEndpoint;
                    break;
                }
            }

            if (endpoint is null)
            {
                throw new InvalidOperationException("This authorization handler can only be applied to controllers routed with endpoint routing");
            }

            var actionDescriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
            var controllerType = actionDescriptor.ControllerTypeInfo.AsType();
            return _sbnFeatures.IsControllerEnabled(controllerType);
        }
    }
}
