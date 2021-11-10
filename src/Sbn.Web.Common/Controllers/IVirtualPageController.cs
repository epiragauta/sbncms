using Microsoft.AspNetCore.Mvc.Filters;
using Sbn.Cms.Core.Models.PublishedContent;

namespace Sbn.Cms.Web.Common.Controllers
{
    /// <summary>
    /// Used for custom routed controllers to execute within the context of Sbn
    /// </summary>
    public interface IVirtualPageController
    {
        /// <summary>
        /// Returns the <see cref="IPublishedContent"/> to use as the current page for the request
        /// </summary>
        IPublishedContent FindContent(ActionExecutingContext actionExecutingContext);
    }
}
