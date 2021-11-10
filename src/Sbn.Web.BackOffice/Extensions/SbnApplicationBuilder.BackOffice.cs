using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.Hosting;
using Sbn.Cms.Web.BackOffice.Middleware;
using Sbn.Cms.Web.BackOffice.Routing;
using Sbn.Cms.Web.Common.ApplicationBuilder;
using Sbn.Cms.Web.Common.Extensions;
using Sbn.Cms.Web.Common.Middleware;

namespace Sbn.Extensions
{
    /// <summary>
    /// <see cref="ISbnEndpointBuilderContext"/> extensions for Sbn
    /// </summary>
    public static partial class SbnApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds all required middleware to run the back office
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static ISbnApplicationBuilderContext UseBackOffice(this ISbnApplicationBuilderContext builder)
        {
            KeepAliveSettings keepAliveSettings = builder.ApplicationServices.GetRequiredService<IOptions<KeepAliveSettings>>().Value;
            IHostingEnvironment hostingEnvironment = builder.ApplicationServices.GetRequiredService<IHostingEnvironment>();
            builder.AppBuilder.Map(
                hostingEnvironment.ToAbsolute(keepAliveSettings.KeepAlivePingUrl),
                a => a.UseMiddleware<KeepAliveMiddleware>());

            builder.AppBuilder.UseMiddleware<BackOfficeExternalLoginProviderErrorMiddleware>();
            return builder;
        }

        public static ISbnEndpointBuilderContext UseBackOfficeEndpoints(this ISbnEndpointBuilderContext app)
        {
            // NOTE: This method will have been called after UseRouting, UseAuthentication, UseAuthorization
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (!app.RuntimeState.SbnCanBoot())
            {
                return app;
            }

            BackOfficeAreaRoutes backOfficeRoutes = app.ApplicationServices.GetRequiredService<BackOfficeAreaRoutes>();
            backOfficeRoutes.CreateRoutes(app.EndpointRouteBuilder);

            app.UseSbnRuntimeMinificationEndpoints();
            app.UseSbnPreviewEndpoints();

            return app;
        }
    }
}
