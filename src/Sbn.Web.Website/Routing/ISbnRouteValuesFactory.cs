using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Sbn.Cms.Core.Routing;
using Sbn.Cms.Web.Common.Routing;

namespace Sbn.Cms.Web.Website.Routing
{
    /// <summary>
    /// Used to create <see cref="SbnRouteValues"/>
    /// </summary>
    public interface ISbnRouteValuesFactory
    {
        /// <summary>
        /// Creates <see cref="SbnRouteValues"/>
        /// </summary>
        Task<SbnRouteValues> CreateAsync(HttpContext httpContext, IPublishedRequest request);
    }
}
