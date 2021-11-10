using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Sbn.Cms.Web.Common.ApplicationBuilder;
using Sbn.Cms.Web.Common.Middleware;
using Sbn.Cms.Web.Website.Routing;

namespace Sbn.Extensions
{
    /// <summary>
    /// <see cref="IApplicationBuilder"/> extensions for the sbn front-end website
    /// </summary>
    public static partial class SbnApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds all required middleware to run the website
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static ISbnApplicationBuilderContext UseWebsite(this ISbnApplicationBuilderContext builder)
        {
            builder.AppBuilder.UseMiddleware<BasicAuthenticationMiddleware>();
            return builder;
        }

        /// <summary>
        /// Sets up routes for the front-end sbn website
        /// </summary>
        public static ISbnEndpointBuilderContext UseWebsiteEndpoints(this ISbnEndpointBuilderContext builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (!builder.RuntimeState.SbnCanBoot())
            {
                return builder;
            }

            FrontEndRoutes surfaceRoutes = builder.ApplicationServices.GetRequiredService<FrontEndRoutes>();
            surfaceRoutes.CreateRoutes(builder.EndpointRouteBuilder);
            builder.EndpointRouteBuilder.MapDynamicControllerRoute<SbnRouteValueTransformer>("/{**slug}");

            return builder;
        }
    }
}
