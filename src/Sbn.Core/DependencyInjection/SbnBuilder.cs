// Copyright (c) Sbn.
// See LICENSE for more details.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Sbn.Cms.Core.Cache;
using Sbn.Cms.Core.Composing;
using Sbn.Cms.Core.Configuration;
using Sbn.Cms.Core.Configuration.Grid;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.Diagnostics;
using Sbn.Cms.Core.Dictionary;
using Sbn.Cms.Core.Editors;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Features;
using Sbn.Cms.Core.Handlers;
using Sbn.Cms.Core.Hosting;
using Sbn.Cms.Core.Install;
using Sbn.Cms.Core.IO;
using Sbn.Cms.Core.Logging;
using Sbn.Cms.Core.Mail;
using Sbn.Cms.Core.Manifest;
using Sbn.Cms.Core.Models.PublishedContent;
using Sbn.Cms.Core.Notifications;
using Sbn.Cms.Core.PropertyEditors;
using Sbn.Cms.Core.PublishedCache;
using Sbn.Cms.Core.PublishedCache.Internal;
using Sbn.Cms.Core.Routing;
using Sbn.Cms.Core.Runtime;
using Sbn.Cms.Core.Security;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Sync;
using Sbn.Cms.Core.Templates;
using Sbn.Cms.Core.Web;
using Sbn.Cms.Web.Common.DependencyInjection;
using Sbn.Extensions;

namespace Sbn.Cms.Core.DependencyInjection
{
    public class SbnBuilder : ISbnBuilder
    {
        private readonly Dictionary<Type, ICollectionBuilder> _builders = new Dictionary<Type, ICollectionBuilder>();

        public IServiceCollection Services { get; }

        public IConfiguration Config { get; }

        public TypeLoader TypeLoader { get; }

        /// <inheritdoc />
        public ILoggerFactory BuilderLoggerFactory { get; }

        /// <inheritdoc />
        public IHostingEnvironment BuilderHostingEnvironment { get; }

        public IProfiler Profiler { get; }

        public AppCaches AppCaches { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SbnBuilder"/> class primarily for testing.
        /// </summary>
        public SbnBuilder(IServiceCollection services, IConfiguration config, TypeLoader typeLoader)
            : this(services, config, typeLoader, NullLoggerFactory.Instance, new NoopProfiler(), AppCaches.Disabled, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SbnBuilder"/> class.
        /// </summary>
        public SbnBuilder(
            IServiceCollection services,
            IConfiguration config,
            TypeLoader typeLoader,
            ILoggerFactory loggerFactory,
            IProfiler profiler,
            AppCaches appCaches,
            IHostingEnvironment hostingEnvironment)
        {
            Services = services;
            Config = config;
            BuilderLoggerFactory = loggerFactory;
            BuilderHostingEnvironment = hostingEnvironment;
            Profiler = profiler;
            AppCaches = appCaches;
            TypeLoader = typeLoader;

            AddCoreServices();
        }

        /// <summary>
        /// Gets a collection builder (and registers the collection).
        /// </summary>
        /// <typeparam name="TBuilder">The type of the collection builder.</typeparam>
        /// <returns>The collection builder.</returns>
        public TBuilder WithCollectionBuilder<TBuilder>()
            where TBuilder : ICollectionBuilder, new()
        {
            Type typeOfBuilder = typeof(TBuilder);

            if (_builders.TryGetValue(typeOfBuilder, out ICollectionBuilder o))
            {
                return (TBuilder)o;
            }

            var builder = new TBuilder();
            _builders[typeOfBuilder] = builder;
            return builder;
        }

        public void Build()
        {
            foreach (ICollectionBuilder builder in _builders.Values)
            {
                builder.RegisterWith(Services);
            }

            _builders.Clear();
        }

        private void AddCoreServices()
        {
            Services.AddSingleton(AppCaches);
            Services.AddSingleton(Profiler);

            // Register as singleton to allow injection everywhere.
            Services.AddSingleton<ServiceFactory>(p => p.GetService);
            Services.AddSingleton<IEventAggregator, EventAggregator>();

            Services.AddLazySupport();

            // Adds no-op registrations as many core services require these dependencies but these
            // dependencies cannot be fulfilled in the Core project
            Services.AddUnique<IMarchal, NoopMarchal>();
            Services.AddUnique<IApplicationShutdownRegistry, NoopApplicationShutdownRegistry>();

            Services.AddUnique<IMainDom, MainDom>();
            Services.AddUnique<IMainDomLock, MainDomSemaphoreLock>();

            Services.AddUnique<IIOHelper>(factory =>
            {
                IHostingEnvironment hostingEnvironment = factory.GetRequiredService<IHostingEnvironment>();

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    return new IOHelperLinux(hostingEnvironment);
                }

                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    return new IOHelperOSX(hostingEnvironment);
                }

                return new IOHelperWindows(hostingEnvironment);
            });

            Services.AddUnique(factory => factory.GetRequiredService<AppCaches>().RuntimeCache);
            Services.AddUnique(factory => factory.GetRequiredService<AppCaches>().RequestCache);
            Services.AddUnique<IProfilingLogger, ProfilingLogger>();
            Services.AddUnique<ISbnVersion, SbnVersion>();

            this.AddAllCoreCollectionBuilders();
            this.AddNotificationHandler<SbnApplicationStartingNotification, EssentialDirectoryCreator>();

            Services.AddSingleton<SbnRequestPaths>();

            Services.AddUnique<InstallStatusTracker>();

            // by default, register a noop factory
            Services.AddUnique<IPublishedModelFactory, NoopPublishedModelFactory>();

            Services.AddUnique<ICultureDictionaryFactory, DefaultCultureDictionaryFactory>();
            Services.AddSingleton(f => f.GetRequiredService<ICultureDictionaryFactory>().CreateDictionary());

            Services.AddUnique<UriUtility>();

            Services.AddUnique<IDashboardService, DashboardService>();
            Services.AddUnique<IUserDataService, UserDataService>();

            // will be injected in controllers when needed to invoke rest endpoints on Our
            Services.AddUnique<IInstallationService, InstallationService>();
            Services.AddUnique<IUpgradeService, UpgradeService>();

            // Grid config is not a real config file as we know them
            Services.AddUnique<IGridConfig, GridConfig>();

            Services.AddUnique<IPublishedUrlProvider, UrlProvider>();
            Services.AddUnique<ISiteDomainMapper, SiteDomainMapper>();

            Services.AddUnique<HtmlLocalLinkParser>();
            Services.AddUnique<HtmlImageSourceParser>();
            Services.AddUnique<HtmlUrlParser>();

            // register properties fallback
            Services.AddUnique<IPublishedValueFallback, PublishedValueFallback>();

            Services.AddUnique<SbnFeatures>();

            // register published router
            Services.AddUnique<IPublishedRouter, PublishedRouter>();

            Services.AddUnique<IEventMessagesFactory, DefaultEventMessagesFactory>();
            Services.AddUnique<IEventMessagesAccessor, HybridEventMessagesAccessor>();
            Services.AddUnique<ITreeService, TreeService>();
            Services.AddUnique<ISectionService, SectionService>();

            Services.AddUnique<ISmsSender, NotImplementedSmsSender>();
            Services.AddUnique<IEmailSender, NotImplementedEmailSender>();

            Services.AddUnique<IDataValueEditorFactory, DataValueEditorFactory>();

            // register distributed cache
            Services.AddUnique(f => new DistributedCache(f.GetRequiredService<IServerMessenger>(), f.GetRequiredService<CacheRefresherCollection>()));
            Services.AddUnique<ICacheRefresherNotificationFactory, CacheRefresherNotificationFactory>();

            // register the http context and sbn context accessors
            // we *should* use the HttpContextSbnContextAccessor, however there are cases when
            // we have no http context, eg when booting Sbn or in background threads, so instead
            // let's use an hybrid accessor that can fall back to a ThreadStatic context.
            Services.AddUnique<ISbnContextAccessor, HybridSbnContextAccessor>();

            Services.AddUnique<LegacyPasswordSecurity>();
            Services.AddUnique<UserEditorAuthorizationHelper>();
            Services.AddUnique<ContentPermissions>();
            Services.AddUnique<MediaPermissions>();

            Services.AddUnique<PropertyEditorCollection>();
            Services.AddUnique<ParameterEditorCollection>();

            // register a server registrar, by default it's the db registrar
            Services.AddUnique<IServerRoleAccessor>(f =>
            {
                GlobalSettings globalSettings = f.GetRequiredService<IOptions<GlobalSettings>>().Value;
                var singleServer = globalSettings.DisableElectionForSingleServer;
                return singleServer
                    ? (IServerRoleAccessor)new SingleServerRoleAccessor()
                    : new ElectedServerRoleAccessor(f.GetRequiredService<IServerRegistrationService>());
            });

            // For Sbn to work it must have the default IPublishedModelFactory
            // which may be replaced by models builder but the default is required to make plain old IPublishedContent
            // instances.
            Services.AddSingleton<IPublishedModelFactory>(factory => factory.CreateDefaultPublishedModelFactory());

            Services
                .AddNotificationHandler<MemberGroupSavedNotification, PublicAccessHandler>()
                .AddNotificationHandler<MemberGroupDeletedNotification, PublicAccessHandler>();

            Services.AddSingleton<ISyncBootStateAccessor, NonRuntimeLevelBootStateAccessor>();

            // register a basic/noop published snapshot service to be replaced
            Services.AddSingleton<IPublishedSnapshotService, InternalPublishedSnapshotService>();

            // Register ValueEditorCache used for validation
            Services.AddSingleton<IValueEditorCache, ValueEditorCache>();
        }
    }
}
