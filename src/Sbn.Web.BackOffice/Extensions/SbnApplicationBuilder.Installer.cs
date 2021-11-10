using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Sbn.Cms.Web.BackOffice.Install;
using Sbn.Cms.Web.Common.ApplicationBuilder;

namespace Sbn.Extensions
{
    /// <summary>
    /// <see cref="IApplicationBuilder"/> extensions for Sbn installer
    /// </summary>
    public static partial class SbnApplicationBuilderExtensions
    {
        /// <summary>
        /// Enables the Sbn installer
        /// </summary>
        public static ISbnEndpointBuilderContext UseInstallerEndpoints(this ISbnEndpointBuilderContext app)
        {
            if (!app.RuntimeState.SbnCanBoot())
            {
                return app;
            }

            InstallAreaRoutes installerRoutes = app.ApplicationServices.GetRequiredService<InstallAreaRoutes>();
            installerRoutes.CreateRoutes(app.EndpointRouteBuilder);

            return app;
        }
    }
}
