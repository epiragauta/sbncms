using Sbn.Cms.Core.Composing;

namespace Sbn.Cms.Web.Common.Controllers
{
    /// <summary>
    /// Provides a base class for auto-routed Sbn API controllers.
    /// </summary>
    public abstract class SbnApiController : SbnApiControllerBase, IDiscoverable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SbnApiController"/> class.
        /// </summary>
        protected SbnApiController()
        {
        }
    }
}
