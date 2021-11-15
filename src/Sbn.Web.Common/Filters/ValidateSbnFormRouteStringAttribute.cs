using System;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Sbn.Cms.Web.Common.Constants;
using Sbn.Cms.Web.Common.Exceptions;
using Sbn.Cms.Web.Common.Security;
using Sbn.Extensions;

namespace Sbn.Cms.Web.Common.Filters
{

    /// <summary>
    /// Attribute used to check that the request contains a valid Sbn form request string.
    /// </summary>
    /// <remarks>
    /// Applying this attribute/filter to a <see cref="SurfaceController"/> or SurfaceController Action will ensure that the Action can only be executed
    /// when it is routed to from within Sbn, typically when rendering a form with BeginSbnForm. It will mean that the natural MVC route for this Action
    /// will fail with a <see cref="HttpSbnFormRouteStringException"/>.
    /// </remarks>
    public class ValidateSbnFormRouteStringAttribute : TypeFilterAttribute
    {

        // TODO: Lets revisit this when we get members done and the front-end working and whether it can moved to an authz policy

        public ValidateSbnFormRouteStringAttribute() : base(typeof(ValidateSbnFormRouteStringFilter))
        {
            Arguments = new object[] { };
        }

        internal class ValidateSbnFormRouteStringFilter: IAuthorizationFilter
        {
            private readonly IDataProtectionProvider _dataProtectionProvider;

            public ValidateSbnFormRouteStringFilter(IDataProtectionProvider dataProtectionProvider)
            {
                _dataProtectionProvider = dataProtectionProvider;
            }

            public void OnAuthorization(AuthorizationFilterContext context)
            {
                if (context == null) throw new ArgumentNullException(nameof(context));

                var ufprt = context.HttpContext.Request.Form["ufprt"];

                if (context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
                {
                    ValidateRouteString(ufprt, controllerActionDescriptor.ControllerName, controllerActionDescriptor.ActionName, context.RouteData?.DataTokens["area"]?.ToString());
                }

            }

            public void ValidateRouteString(string ufprt, string currentController, string currentAction, string currentArea)
            {
                if (ufprt.IsNullOrWhiteSpace())
                {
                    throw new HttpSbnFormRouteStringException("The required request field \"ufprt\" is not present.");
                }

                if (!EncryptionHelper.DecryptAndValidateEncryptedRouteString(_dataProtectionProvider, ufprt, out var additionalDataParts))
                {
                    throw new HttpSbnFormRouteStringException("The Sbn form request route string could not be decrypted.");
                }

                if (!additionalDataParts[ViewConstants.ReservedAdditionalKeys.Controller].InvariantEquals(currentController) ||
                    !additionalDataParts[ViewConstants.ReservedAdditionalKeys.Action].InvariantEquals(currentAction) ||
                    (!additionalDataParts[ViewConstants.ReservedAdditionalKeys.Area].IsNullOrWhiteSpace() && !additionalDataParts[ViewConstants.ReservedAdditionalKeys.Area].InvariantEquals(currentArea)))
                {
                    throw new HttpSbnFormRouteStringException("The provided Sbn form request route string was meant for a different controller and action.");
                }

            }
        }
    }
}
