using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sbn.Cms.Core.Features;
using Sbn.Cms.Web.Common.Attributes;
using Sbn.Cms.Web.Common.Authorization;

namespace Sbn.Cms.Web.Common.Controllers
{
    /// <summary>
    /// Provides a base class for Sbn API controllers.
    /// </summary>
    /// <remarks>
    /// <para>These controllers are NOT auto-routed.</para>
    /// <para>The base class is <see cref="ControllerBase"/> which are netcore API controllers without any view support</para>
    /// </remarks>
    [Authorize(Policy = AuthorizationPolicies.SbnFeatureEnabled)] // TODO: This could be part of our conventions
    [SbnApiController]
    public abstract class SbnApiControllerBase : ControllerBase, ISbnFeature
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SbnApiControllerBase"/> class.
        /// </summary>
        protected SbnApiControllerBase()
        {
        }
    }
}
