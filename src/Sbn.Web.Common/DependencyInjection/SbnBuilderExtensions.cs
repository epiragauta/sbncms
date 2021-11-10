using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using Dazinator.Extensions.FileProviders.GlobPatternFilter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Smidge;
using Smidge.FileProcessors;
using Smidge.InMemory;
using Smidge.Nuglify;
using Sbn.Cms.Core;
using Sbn.Cms.Core.Cache;
using Sbn.Cms.Core.Composing;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.DependencyInjection;
using Sbn.Cms.Core.Diagnostics;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Hosting;
using Sbn.Cms.Core.Logging;
using Sbn.Cms.Core.Macros;
using Sbn.Cms.Core.Net;
using Sbn.Cms.Core.Notifications;
using Sbn.Cms.Core.Security;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Templates;
using Sbn.Cms.Core.Web;
using Sbn.Cms.Core.WebAssets;
using Sbn.Cms.Infrastructure.DependencyInjection;
using Sbn.Cms.Infrastructure.HostedServices;
using Sbn.Cms.Infrastructure.HostedServices.ServerRegistration;
using Sbn.Cms.Infrastructure.Migrations.Install;
using Sbn.Cms.Infrastructure.Persistence;
using Sbn.Cms.Infrastructure.Persistence.SqlSyntax;
using Sbn.Cms.Web.Common;
using Sbn.Cms.Web.Common.ApplicationModels;
using Sbn.Cms.Web.Common.AspNetCore;
using Sbn.Cms.Web.Common.Controllers;
using Sbn.Cms.Web.Common.DependencyInjection;
using Sbn.Cms.Web.Common.Localization;
using Sbn.Cms.Web.Common.Macros;
using Sbn.Cms.Web.Common.Middleware;
using Sbn.Cms.Web.Common.ModelBinders;
using Sbn.Cms.Web.Common.Mvc;
using Sbn.Cms.Web.Common.Profiler;
using Sbn.Cms.Web.Common.RuntimeMinification;
using Sbn.Cms.Web.Common.Security;
using Sbn.Cms.Web.Common.Templates;
using Sbn.Cms.Web.Common.SbnContext;
using IHostingEnvironment = Sbn.Cms.Core.Hosting.IHostingEnvironment;

namespace Sbn.Extensions
{
    // TODO: We could add parameters to configure each of these for flexibility

    /// <summary>
    /// Extension methods for <see cref="ISbnBuilder"/> for the common Sbn functionality
    /// </summary>
    public static partial class SbnBuilderExtensions
    {
        /// <summary>
        /// Creates an <see cref="ISbnBuilder"/> and registers basic Sbn services
        /// </summary>
        public static ISbnBuilder AddSbn(
            this IServiceCollection services,
            IWebHostEnvironment webHostEnvironment,
            IConfiguration config)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            IHostingEnvironment tempHostingEnvironment = GetTemporaryHostingEnvironment(webHostEnvironment, config);

            var loggingDir = tempHostingEnvironment.MapPathContentRoot(Constants.SystemDirectories.LogFiles);
            var loggingConfig = new LoggingConfiguration(loggingDir);

            services.AddLogger(tempHostingEnvironment, loggingConfig, config);

            // The DataDirectory is used to resolve database file paths (directly supported by SQL CE and manually replaced for LocalDB)
            AppDomain.CurrentDomain.SetData("DataDirectory", tempHostingEnvironment?.MapPathContentRoot(Constants.SystemDirectories.Data));

            // Manually create and register the HttpContextAccessor. In theory this should not be registered
            // again by the user but if that is the case it's not the end of the world since HttpContextAccessor
            // is just based on AsyncLocal, see https://github.com/dotnet/aspnetcore/blob/main/src/Http/Http/src/HttpContextAccessor.cs
            IHttpContextAccessor httpContextAccessor = new HttpContextAccessor();
            services.AddSingleton(httpContextAccessor);

            var requestCache = new HttpContextRequestAppCache(httpContextAccessor);
            var appCaches = AppCaches.Create(requestCache);

            services.ConfigureOptions<ConfigureKestrelServerOptions>();
            services.ConfigureOptions<ConfigureIISServerOptions>();
            services.ConfigureOptions<ConfigureFormOptions>();

            IProfiler profiler = GetWebProfiler(config);

            ILoggerFactory loggerFactory = LoggerFactory.Create(cfg => cfg.AddSerilog(Log.Logger, false));

            TypeLoader typeLoader = services.AddTypeLoader(
                Assembly.GetEntryAssembly(),
                tempHostingEnvironment,
                loggerFactory,
                appCaches,
                config,
                profiler);

            return new SbnBuilder(services, config, typeLoader, loggerFactory, profiler, appCaches, tempHostingEnvironment);
        }

        /// <summary>
        /// Adds core Sbn services
        /// </summary>
        /// <remarks>
        /// This will not add any composers/components
        /// </remarks>
        public static ISbnBuilder AddSbnCore(this ISbnBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            // Add ASP.NET specific services
            builder.Services.AddUnique<IBackOfficeInfo, AspNetCoreBackOfficeInfo>();
            builder.Services.AddUnique<IHostingEnvironment, AspNetCoreHostingEnvironment>();
            builder.Services.AddHostedService(factory => factory.GetRequiredService<IRuntime>());

            // Add supported databases
            builder.AddSbnSqlServerSupport();
            builder.AddSbnSqlCeSupport();
            builder.Services.AddUnique<DatabaseSchemaCreatorFactory>();

            // Must be added here because DbProviderFactories is netstandard 2.1 so cannot exist in Infra for now
            builder.Services.AddSingleton<IDbProviderFactoryCreator>(factory => new DbProviderFactoryCreator(
                DbProviderFactories.GetFactory,
                factory.GetServices<ISqlSyntaxProvider>(),
                factory.GetServices<IBulkSqlInsertProvider>(),
                factory.GetServices<IDatabaseCreator>(),
                factory.GetServices<IProviderSpecificMapperFactory>()
            ));

            builder.AddCoreInitialServices();

            // aspnet app lifetime mgmt
            builder.Services.AddUnique<ISbnApplicationLifetime, AspNetCoreSbnApplicationLifetime>();
            builder.Services.AddUnique<IApplicationShutdownRegistry, AspNetCoreApplicationShutdownRegistry>();

            return builder;
        }

        /// <summary>
        /// Add Sbn hosted services
        /// </summary>
        public static ISbnBuilder AddHostedServices(this ISbnBuilder builder)
        {
            builder.Services.AddHostedService<QueuedHostedService>();
            builder.Services.AddHostedService<HealthCheckNotifier>();
            builder.Services.AddHostedService<KeepAlive>();
            builder.Services.AddHostedService<LogScrubber>();
            builder.Services.AddHostedService<ScheduledPublishing>();
            builder.Services.AddHostedService<TempFileCleanup>();
            builder.Services.AddHostedService<InstructionProcessTask>();
            builder.Services.AddHostedService<TouchServerTask>();
            builder.Services.AddHostedService<ReportSiteTask>();
            return builder;
        }

        private static ISbnBuilder AddHttpClients(this ISbnBuilder builder)
        {
            builder.Services.AddHttpClient();
            return builder;
        }

        /// <summary>
        /// Adds the Sbn request profiler
        /// </summary>
        public static ISbnBuilder AddSbnProfiler(this ISbnBuilder builder)
        {
            builder.Services.AddUnique<WebProfilerHtml>();

            builder.Services.AddMiniProfiler(options =>
            {
                // WebProfiler determine and start profiling. We should not use the MiniProfilerMiddleware to also profile
                options.ShouldProfile = request => false;

                // this is a default path and by default it performs a 'contains' check which will match our content controller
                // (and probably other requests) and ignore them.
                options.IgnoredPaths.Remove("/content/");
            });

            builder.AddNotificationHandler<SbnApplicationStartingNotification, InitializeWebProfiling>();
            return builder;
        }

        public static ISbnBuilder AddMvcAndRazor(this ISbnBuilder builder, Action<IMvcBuilder> mvcBuilding = null)
        {
            // TODO: We need to figure out if we can work around this because calling AddControllersWithViews modifies the global app and order is very important
            // this will directly affect developers who need to call that themselves.
            // We need to have runtime compilation of views when using sbn. We could consider having only this when a specific config is set.
            // But as far as I can see, there are still precompiled views, even when this is activated, so maybe it is okay.
            IMvcBuilder mvcBuilder = builder.Services
                .AddControllersWithViews();

            FixForDotnet6Preview1(builder.Services);
            mvcBuilder.AddRazorRuntimeCompilation();

            mvcBuilding?.Invoke(mvcBuilder);

            return builder;
        }

        /// <summary>
        /// This fixes an issue for .NET6 Preview1, that in AddRazorRuntimeCompilation cannot remove the existing IViewCompilerProvider.
        /// </summary>
        /// <remarks>
        ///  When running .NET6 Preview1 there is an issue with looks to be fixed when running ASP.NET Core 6.
        ///  This issue is because the default implementation of IViewCompilerProvider has changed, so the
        ///  AddRazorRuntimeCompilation extension can't remove the default and replace with the runtimeviewcompiler.
        ///
        ///  This method basically does the same as the ASP.NET Core 6 version of AddRazorRuntimeCompilation
        ///  https://github.com/dotnet/aspnetcore/blob/f7dc5e24af7f9692a1db66741954b90b42d84c3a/src/Mvc/Mvc.Razor.RuntimeCompilation/src/DependencyInjection/RazorRuntimeCompilationMvcCoreBuilderExtensions.cs#L71-L80
        ///
        ///  While running .NET5 this does nothing as the ImplementationType has another FullName, and this is handled by the .NET5 version of AddRazorRuntimeCompilation
        /// </remarks>
        private static void FixForDotnet6Preview1(IServiceCollection services)
        {
            var compilerProvider = services.FirstOrDefault(f =>
                f.ServiceType == typeof(IViewCompilerProvider) &&
                f.ImplementationType?.Assembly == typeof(IViewCompilerProvider).Assembly &&
                f.ImplementationType.FullName == "Microsoft.AspNetCore.Mvc.Razor.Compilation.DefaultViewCompiler");

            if (compilerProvider != null)
            {
                services.Remove(compilerProvider);
            }
        }

        /// <summary>
        /// Add runtime minifier support for Sbn
        /// </summary>
        public static ISbnBuilder AddRuntimeMinifier(this ISbnBuilder builder)
        {
            // Add custom ISmidgeFileProvider to include the additional App_Plugins location
            // to load assets from.
            builder.Services.AddSingleton<ISmidgeFileProvider>(f =>
            {
                IWebHostEnvironment hostEnv = f.GetRequiredService<IWebHostEnvironment>();

                return new SmidgeFileProvider(
                    hostEnv.WebRootFileProvider,
                    new GlobPatternFilterFileProvider(
                        hostEnv.ContentRootFileProvider,
                        // only include js or css files within App_Plugins
                        new[] { "/App_Plugins/**/*.js", "/App_Plugins/**/*.css" }));
            });

            builder.Services.AddSmidge(builder.Config.GetSection(Constants.Configuration.ConfigRuntimeMinification));
            builder.Services.AddSmidgeNuglify();
            builder.Services.AddSmidgeInMemory(false); // it will be enabled based on config/cachebuster

            builder.Services.AddUnique<IRuntimeMinifier, SmidgeRuntimeMinifier>();
            builder.Services.AddUnique<SmidgeHelperAccessor>();
            builder.Services.AddTransient<IPreProcessor, SmidgeNuglifyJs>();
            builder.Services.ConfigureOptions<SmidgeOptionsSetup>();

            return builder;
        }

        /// <summary>
        /// Adds all web based services required for Sbn to run
        /// </summary>
        public static ISbnBuilder AddWebComponents(this ISbnBuilder builder)
        {
            // Add service session
            // This can be overwritten by the user by adding their own call to AddSession
            // since the last call of AddSession take precedence
            builder.Services.AddSession(options =>
            {
                options.Cookie.Name = "UMB_SESSION";
                options.Cookie.HttpOnly = true;
            });

            builder.Services.ConfigureOptions<SbnMvcConfigureOptions>();
            builder.Services.ConfigureOptions<SbnRequestLocalizationOptions>();
            builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IApplicationModelProvider, SbnApiBehaviorApplicationModelProvider>());
            builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IApplicationModelProvider, BackOfficeApplicationModelProvider>());
            builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IApplicationModelProvider, VirtualPageApplicationModelProvider>());
            builder.AddSbnImageSharp();

            // AspNetCore specific services
            builder.Services.AddUnique<IRequestAccessor, AspNetCoreRequestAccessor>();
            builder.AddNotificationHandler<SbnRequestBeginNotification, AspNetCoreRequestAccessor>();

            // Password hasher
            builder.Services.AddUnique<IPasswordHasher, AspNetCorePasswordHasher>();

            builder.Services.AddUnique<Cms.Core.Web.ICookieManager, AspNetCoreCookieManager>();
            builder.Services.AddTransient<IIpResolver, AspNetCoreIpResolver>();
            builder.Services.AddUnique<IUserAgentProvider, AspNetCoreUserAgentProvider>();

            builder.Services.AddMultipleUnique<ISessionIdResolver, ISessionManager, AspNetCoreSessionManager>();

            builder.Services.AddUnique<IMarchal, AspNetCoreMarchal>();

            builder.Services.AddUnique<IProfilerHtml, WebProfilerHtml>();

            builder.Services.AddUnique<IMacroRenderer, MacroRenderer>();
            builder.Services.AddUnique<PartialViewMacroEngine>();

            // register the sbn context factory

            builder.Services.AddUnique<ISbnContextFactory, SbnContextFactory>();
            builder.Services.AddUnique<IBackOfficeSecurityAccessor, BackOfficeSecurityAccessor>();

            var sbnApiControllerTypes = builder.TypeLoader.GetSbnApiControllers().ToList();
            builder.WithCollectionBuilder<SbnApiControllerTypeCollectionBuilder>()
                .Add(sbnApiControllerTypes);

            builder.Services.AddUnique<SbnRequestLoggingMiddleware>();
            builder.Services.AddUnique<PreviewAuthenticationMiddleware>();
            builder.Services.AddUnique<SbnRequestMiddleware>();
            builder.Services.AddUnique<BootFailedMiddleware>();

            builder.Services.AddUnique<SbnJsonModelBinder>();

            builder.Services.AddUnique<ITemplateRenderer, TemplateRenderer>();
            builder.Services.AddUnique<IPublicAccessChecker, PublicAccessChecker>();

            builder.Services.AddSingleton<ContentModelBinder>();

            builder.Services.AddSingleton<ISbnHelperAccessor, SbnHelperAccessor>();
            builder.Services.AddScoped<SbnHelper>();
            builder.Services.AddScoped<IBackOfficeSecurity, BackOfficeSecurity>();

            builder.AddHttpClients();

            return builder;
        }


        // TODO: Does this need to exist and/or be public?
        public static ISbnBuilder AddWebServer(this ISbnBuilder builder)
        {
            // TODO: We need to figure out why this is needed and fix those endpoints to not need them, we don't want to change global things
            // If using Kestrel: https://stackoverflow.com/a/55196057
            builder.Services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
            builder.Services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            return builder;
        }

        /// <summary>
        /// Adds SqlCe support for Sbn
        /// </summary>
        private static ISbnBuilder AddSbnSqlCeSupport(this ISbnBuilder builder)
        {
            try
            {
                var binFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                if (binFolder != null)
                {
                    var dllPath = Path.Combine(binFolder, "Sbn.Persistence.SqlCe.dll");
                    var umbSqlCeAssembly = Assembly.LoadFrom(dllPath);

                    Type sqlCeSyntaxProviderType = umbSqlCeAssembly.GetType("Sbn.Cms.Persistence.SqlCe.SqlCeSyntaxProvider");
                    Type sqlCeBulkSqlInsertProviderType = umbSqlCeAssembly.GetType("Sbn.Cms.Persistence.SqlCe.SqlCeBulkSqlInsertProvider");
                    Type sqlCeDatabaseCreatorType = umbSqlCeAssembly.GetType("Sbn.Cms.Persistence.SqlCe.SqlCeDatabaseCreator");
                    Type sqlCeSpecificMapperFactory = umbSqlCeAssembly.GetType("Sbn.Cms.Persistence.SqlCe.SqlCeSpecificMapperFactory");

                    if (!(sqlCeSyntaxProviderType is null
                          || sqlCeBulkSqlInsertProviderType is null
                          || sqlCeDatabaseCreatorType is null
                          || sqlCeSpecificMapperFactory is null))
                    {
                        builder.Services.AddSingleton(typeof(ISqlSyntaxProvider), sqlCeSyntaxProviderType);
                        builder.Services.AddSingleton(typeof(IBulkSqlInsertProvider), sqlCeBulkSqlInsertProviderType);
                        builder.Services.AddSingleton(typeof(IDatabaseCreator), sqlCeDatabaseCreatorType);
                        builder.Services.AddSingleton(typeof(IProviderSpecificMapperFactory), sqlCeSpecificMapperFactory);
                    }

                    var sqlCeAssembly = Assembly.LoadFrom(Path.Combine(binFolder, "System.Data.SqlServerCe.dll"));

                    var sqlCe = sqlCeAssembly.GetType("System.Data.SqlServerCe.SqlCeProviderFactory");
                    if (!(sqlCe is null))
                    {
                        DbProviderFactories.RegisterFactory(Cms.Core.Constants.DbProviderNames.SqlCe, sqlCe);
                    }
                }
            }
            catch
            {
                // Ignore if SqlCE is not available
            }

            return builder;
        }

        /// <summary>
        /// Adds Sql Server support for Sbn
        /// </summary>
        private static ISbnBuilder AddSbnSqlServerSupport(this ISbnBuilder builder)
        {
            DbProviderFactories.RegisterFactory(Cms.Core.Constants.DbProviderNames.SqlServer, SqlClientFactory.Instance);

            builder.Services.AddSingleton<ISqlSyntaxProvider, SqlServerSyntaxProvider>();
            builder.Services.AddSingleton<IBulkSqlInsertProvider, SqlServerBulkSqlInsertProvider>();
            builder.Services.AddSingleton<IDatabaseCreator, SqlServerDatabaseCreator>();

            return builder;
        }

        private static IProfiler GetWebProfiler(IConfiguration config)
        {
            var isDebug = config.GetValue<bool>($"{Cms.Core.Constants.Configuration.ConfigHosting}:Debug");
            // create and start asap to profile boot
            if (!isDebug)
            {
                // should let it be null, that's how MiniProfiler is meant to work,
                // but our own IProfiler expects an instance so let's get one
                return new NoopProfiler();
            }

            var webProfiler = new WebProfiler();
            webProfiler.StartBoot();

            return webProfiler;
        }

        /// <summary>
        /// HACK: returns an AspNetCoreHostingEnvironment that doesn't monitor changes to configuration.<br/>
        /// We require this to create a TypeLoader during ConfigureServices.<br/>
        /// Instances returned from this method shouldn't be registered in the service collection.
        /// </summary>
        private static IHostingEnvironment GetTemporaryHostingEnvironment(IWebHostEnvironment webHostEnvironment, IConfiguration config)
        {
            var hostingSettings = config.GetSection(Cms.Core.Constants.Configuration.ConfigHosting).Get<HostingSettings>() ?? new HostingSettings();
            var webRoutingSettings = config.GetSection(Cms.Core.Constants.Configuration.ConfigWebRouting).Get<WebRoutingSettings>() ?? new WebRoutingSettings();
            var wrappedHostingSettings = new OptionsMonitorAdapter<HostingSettings>(hostingSettings);
            var wrappedWebRoutingSettings = new OptionsMonitorAdapter<WebRoutingSettings>(webRoutingSettings);

            // This is needed in order to create a unique Application Id
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDataProtection();
            serviceCollection.AddSingleton<IHostEnvironment>(s => webHostEnvironment);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            return new AspNetCoreHostingEnvironment(
                serviceProvider,
                wrappedHostingSettings,
                wrappedWebRoutingSettings,
                webHostEnvironment);
        }

    }
}
