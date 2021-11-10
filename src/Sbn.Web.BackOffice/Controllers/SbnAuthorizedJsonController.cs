using Sbn.Cms.Web.BackOffice.Filters;
using Sbn.Cms.Web.Common.Filters;

namespace Sbn.Cms.Web.BackOffice.Controllers
{
    /// <summary>
    /// An abstract API controller that only supports JSON and all requests must contain the correct csrf header
    /// </summary>
    /// <remarks>
    /// Inheriting from this controller means that ALL of your methods are JSON methods that are called by Angular,
    /// methods that are not called by Angular or don't contain a valid csrf header will NOT work.
    /// </remarks>
    [ValidateAngularAntiForgeryToken]
    public abstract class SbnAuthorizedJsonController : SbnAuthorizedApiController
    {
    }
}
