using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sbn.Cms.Web.Common.Authorization;
using Sbn.Cms.Web.Common.Filters;

namespace Sbn.Cms.Web.Common.Controllers
{
    /// <summary>
    /// Provides a base class for backoffice authorized controllers.
    /// </summary>
    [Authorize(Policy = AuthorizationPolicies.BackOfficeAccess)]
    [DisableBrowserCache]
    public abstract class SbnAuthorizedController : ControllerBase
    {
    }
}
