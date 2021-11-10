using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog.Context;
using StackExchange.Profiling;
using Sbn.Cms.Core;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.Hosting;
using Sbn.Cms.Core.Logging.Serilog.Enrichers;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Web.Common.ApplicationBuilder;
using Sbn.Cms.Web.Common.Middleware;
using Sbn.Cms.Web.Common.Plugins;

namespace Sbn.Extensions
{
    /// <summary>
    /// <see cref="IApplicationBuilder"/> extensions for Sbn
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Configures and use services required for using Sbn
        /// </summary>
        public static ISbnApplicationBuilder UseSbn(this IApplicationBuilder app)
            => new SbnApplicationBuilder(app);

        /// <summary>
        /// Returns true if Sbn <see cref="IRuntimeState"/> is greater than <see cref="RuntimeLevel.BootFailed"/>
        /// </summary>
        public static bool SbnCanBoot(this IApplicationBuilder app)
            => app.ApplicationServices.GetRequiredService<IRuntimeState>().SbnCanBoot();

        /// <summary>
        /// Enables core Sbn functionality
        /// </summary>
        public static IApplicationBuilder UseSbnCore(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (!app.SbnCanBoot())
            {
                return app;
            }

            // Register our global threadabort enricher for logging
            ThreadAbortExceptionEnricher threadAbortEnricher = app.ApplicationServices.GetRequiredService<ThreadAbortExceptionEnricher>();
            LogContext.Push(threadAbortEnricher); // NOTE: We are not in a using clause because we are not removing it, it is on the global context

            return app;
        }

        /// <summary>
        /// Enables middlewares required to run Sbn
        /// </summary>
        /// <remarks>
        /// Must occur before UseRouting
        /// </remarks>
        public static IApplicationBuilder UseSbnRouting(this IApplicationBuilder app)
        {
            // TODO: This method could be internal or part of another call - this is a required system so should't be 'opt-in'
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (!app.SbnCanBoot())
            {
                app.UseStaticFiles(); // We need static files to show the nice error page.
                app.UseMiddleware<BootFailedMiddleware>();
            }
            else
            {
                app.UseMiddleware<PreviewAuthenticationMiddleware>();
                app.UseMiddleware<SbnRequestMiddleware>();
                app.UseMiddleware<MiniProfilerMiddleware>();
            }

            return app;
        }

        /// <summary>
        /// Adds request based serilog enrichers to the LogContext for each request
        /// </summary>
        public static IApplicationBuilder UseSbnRequestLogging(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (!app.SbnCanBoot()) return app;

            app.UseMiddleware<SbnRequestLoggingMiddleware>();

            return app;
        }

        /// <summary>
        /// Allow static file access for App_Plugins folders
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSbnPluginsStaticFiles(this IApplicationBuilder app)
        {
            var hostingEnvironment = app.ApplicationServices.GetRequiredService<IHostingEnvironment>();
            var sbnPluginSettings = app.ApplicationServices.GetRequiredService<IOptions<SbnPluginSettings>>();

            var pluginFolder = hostingEnvironment.MapPathContentRoot(Constants.SystemDirectories.AppPlugins);

            // Ensure the plugin folder exists
            Directory.CreateDirectory(pluginFolder);

            var fileProvider = new SbnPluginPhysicalFileProvider(
                pluginFolder,
                sbnPluginSettings);

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = fileProvider,
                RequestPath = Constants.SystemDirectories.AppPlugins
            });

            return app;
        }
    }

}
