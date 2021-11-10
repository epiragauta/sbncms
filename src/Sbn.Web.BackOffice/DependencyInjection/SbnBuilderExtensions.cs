using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sbn.Cms.Core.Cache;
using Sbn.Cms.Core.DependencyInjection;
using Sbn.Cms.Core.Hosting;
using Sbn.Cms.Core.IO;
using Sbn.Cms.Core.Notifications;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Infrastructure.DependencyInjection;
using Sbn.Cms.Infrastructure.Examine.DependencyInjection;
using Sbn.Cms.Infrastructure.WebAssets;
using Sbn.Cms.Web.BackOffice.Controllers;
using Sbn.Cms.Web.BackOffice.Filters;
using Sbn.Cms.Web.BackOffice.Install;
using Sbn.Cms.Web.BackOffice.Middleware;
using Sbn.Cms.Web.BackOffice.ModelsBuilder;
using Sbn.Cms.Web.BackOffice.Routing;
using Sbn.Cms.Web.BackOffice.Security;
using Sbn.Cms.Web.BackOffice.Services;
using Sbn.Cms.Web.BackOffice.SignalR;
using Sbn.Cms.Web.BackOffice.Trees;

namespace Sbn.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="ISbnBuilder"/> for the Sbn back office
    /// </summary>
    public static partial class SbnBuilderExtensions
    {
        /// <summary>
        /// Adds all required components to run the Sbn back office
        /// </summary>
        public static ISbnBuilder AddBackOffice(this ISbnBuilder builder, Action<IMvcBuilder> configureMvc = null) => builder
                .AddConfiguration()
                .AddSbnCore()
                .AddWebComponents()
                .AddRuntimeMinifier()
                .AddBackOfficeCore()
                .AddBackOfficeAuthentication()
                .AddBackOfficeIdentity()
                .AddMembersIdentity()
                .AddBackOfficeAuthorizationPolicies()
                .AddSbnProfiler()
                .AddMvcAndRazor(configureMvc)
                .AddWebServer()
                .AddPreviewSupport()
                .AddHostedServices()
                .AddNuCache()
                .AddDistributedCache()
                .AddModelsBuilderDashboard()
                .AddUnattendedInstallInstallCreateUser()
                .AddCoreNotifications()
                .AddLogViewer()
                .AddExamine()
                .AddExamineIndexes();

        public static ISbnBuilder AddUnattendedInstallInstallCreateUser(this ISbnBuilder builder)
        {
            builder.AddNotificationAsyncHandler<UnattendedInstallNotification, CreateUnattendedUserNotificationHandler>();
            return builder;
        }

        /// <summary>
        /// Adds Sbn preview support
        /// </summary>
        public static ISbnBuilder AddPreviewSupport(this ISbnBuilder builder)
        {
            builder.Services.AddSignalR();

            return builder;
        }

        /// <summary>
        /// Gets the back office tree collection builder
        /// </summary>
        public static TreeCollectionBuilder Trees(this ISbnBuilder builder)
            => builder.WithCollectionBuilder<TreeCollectionBuilder>();

        public static ISbnBuilder AddBackOfficeCore(this ISbnBuilder builder)
        {
            builder.Services.AddSingleton<KeepAliveMiddleware>();
            builder.Services.ConfigureOptions<ConfigureGlobalOptionsForKeepAliveMiddlware>();
            builder.Services.AddUnique<ServerVariablesParser>();
            builder.Services.AddUnique<InstallAreaRoutes>();
            builder.Services.AddUnique<BackOfficeAreaRoutes>();
            builder.Services.AddUnique<PreviewRoutes>();
            builder.AddNotificationAsyncHandler<ContentCacheRefresherNotification, PreviewHubUpdater>();
            builder.Services.AddUnique<BackOfficeServerVariables>();
            builder.Services.AddScoped<BackOfficeSessionIdValidator>();
            builder.Services.AddScoped<BackOfficeSecurityStampValidator>();

            // register back office trees
            // the collection builder only accepts types inheriting from TreeControllerBase
            // and will filter out those that are not attributed with TreeAttribute
            var sbnApiControllerTypes = builder.TypeLoader.GetSbnApiControllers().ToList();
            builder.Trees()
                .AddTreeControllers(sbnApiControllerTypes.Where(x => typeof(TreeControllerBase).IsAssignableFrom(x)));

            builder.AddWebMappingProfiles();

            builder.Services.AddUnique<IPhysicalFileSystem>(factory =>
            {
                var path = "~/";
                var hostingEnvironment = factory.GetRequiredService<IHostingEnvironment>();
                return new PhysicalFileSystem(
                    factory.GetRequiredService<IIOHelper>(),
                    hostingEnvironment,
                    factory.GetRequiredService<ILogger<PhysicalFileSystem>>(),
                    hostingEnvironment.MapPathContentRoot(path),
                    hostingEnvironment.ToAbsolute(path)
                );
            });

            builder.Services.AddUnique<IIconService, IconService>();
            builder.Services.AddUnique<UnhandledExceptionLoggerMiddleware>();

            return builder;
        }
    }
}
