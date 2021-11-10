using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Sbn.Cms.Web.BackOffice.Routing;
using Sbn.Cms.Web.Common.ApplicationBuilder;

namespace Sbn.Extensions
{
    /// <summary>
    /// <see cref="ISbnEndpointBuilderContext"/> extensions for Sbn
    /// </summary>
    public static partial class SbnApplicationBuilderExtensions
    {
        public static ISbnEndpointBuilderContext UseSbnPreviewEndpoints(this ISbnEndpointBuilderContext app)
        {
            PreviewRoutes previewRoutes = app.ApplicationServices.GetRequiredService<PreviewRoutes>();
            previewRoutes.CreateRoutes(app.EndpointRouteBuilder);

            return app;
        }
    }
}
