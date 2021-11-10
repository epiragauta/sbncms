using Microsoft.AspNetCore.Routing;

namespace Sbn.Cms.Web.Common.ApplicationBuilder
{

    /// <summary>
    /// A builder to allow encapsulating the enabled routing features in Sbn
    /// </summary>
    public interface ISbnEndpointBuilderContext : ISbnApplicationBuilderServices
    {
        IEndpointRouteBuilder EndpointRouteBuilder { get; }
    }
}
