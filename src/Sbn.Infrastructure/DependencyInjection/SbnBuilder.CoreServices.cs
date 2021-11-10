using System.Runtime.InteropServices;
using Examine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using Sbn.Cms.Core;
using Sbn.Cms.Core.Cache;
using Sbn.Cms.Core.Configuration;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.DependencyInjection;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Handlers;
using Sbn.Cms.Core.HealthChecks.NotificationMethods;
using Sbn.Cms.Core.Hosting;
using Sbn.Cms.Core.Install;
using Sbn.Cms.Core.Logging;
using Sbn.Cms.Core.Logging.Serilog.Enrichers;
using Sbn.Cms.Core.Logging.Viewer;
using Sbn.Cms.Core.Mail;
using Sbn.Cms.Core.Manifest;
using Sbn.Cms.Core.Media;
using Sbn.Cms.Core.Migrations;
using Sbn.Cms.Core.Models.PublishedContent;
using Sbn.Cms.Core.Notifications;
using Sbn.Cms.Core.Packaging;
using Sbn.Cms.Core.PropertyEditors;
using Sbn.Cms.Core.PropertyEditors.ValueConverters;
using Sbn.Cms.Core.PublishedCache;
using Sbn.Cms.Core.Routing;
using Sbn.Cms.Core.Runtime;
using Sbn.Cms.Core.Scoping;
using Sbn.Cms.Core.Serialization;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Strings;
using Sbn.Cms.Core.Templates;
using Sbn.Cms.Core.Trees;
using Sbn.Cms.Core.Web;
using Sbn.Cms.Infrastructure.Examine;
using Sbn.Cms.Infrastructure.HealthChecks;
using Sbn.Cms.Infrastructure.HostedServices;
using Sbn.Cms.Infrastructure.Install;
using Sbn.Cms.Infrastructure.Mail;
using Sbn.Cms.Infrastructure.Media;
using Sbn.Cms.Infrastructure.Migrations;
using Sbn.Cms.Infrastructure.Migrations.Install;
using Sbn.Cms.Infrastructure.Migrations.PostMigrations;
using Sbn.Cms.Infrastructure.Packaging;
using Sbn.Cms.Infrastructure.Migrations.Upgrade.V_8_0_0.DataTypes;
using Sbn.Cms.Infrastructure.Persistence;
using Sbn.Cms.Infrastructure.Persistence.Mappers;
using Sbn.Cms.Infrastructure.Runtime;
using Sbn.Cms.Infrastructure.Search;
using Sbn.Cms.Infrastructure.Serialization;
using Sbn.Extensions;

namespace Sbn.Cms.Infrastructure.DependencyInjection
{
    public static partial class SbnBuilderExtensions
    {
        /// <summary>
        /// Adds all core Sbn services required to run which may be replaced later in the pipeline
        /// </summary>
        public static ISbnBuilder AddCoreInitialServices(this ISbnBuilder builder)
        {
            builder
                .AddMainDom()
                .AddLogging();

            builder.Services.AddSingleton<ISbnDatabaseFactory, SbnDatabaseFactory>();
            builder.Services.AddSingleton(factory => factory.GetRequiredService<ISbnDatabaseFactory>().CreateDatabase());
            builder.Services.AddSingleton(factory => factory.GetRequiredService<ISbnDatabaseFactory>().SqlContext);
            builder.NPocoMappers().Add<NullableDateMapper>();
            builder.PackageMigrationPlans().Add(() => builder.TypeLoader.GetPackageMigrationPlans());

            builder.Services.AddSingleton<IRuntimeState, RuntimeState>();
            builder.Services.AddSingleton<IRuntime, CoreRuntime>();
            builder.Services.AddSingleton<PendingPackageMigrations>();
            builder.AddNotificationAsyncHandler<RuntimeUnattendedInstallNotification, UnattendedInstaller>();
            builder.AddNotificationAsyncHandler<RuntimeUnattendedUpgradeNotification, UnattendedUpgrader>();

            // composers
            builder
                .AddRepositories()
                .AddServices()
                .AddCoreMappingProfiles()
                .AddFileSystems()
                .AddWebAssets();

            // register persistence mappers - required by database factory so needs to be done here
            // means the only place the collection can be modified is in a runtime - afterwards it
            // has been frozen and it is too late
            builder.Mappers().AddCoreMappers();

            // register the scope provider
            builder.Services.AddSingleton<ScopeProvider>(); // implements both IScopeProvider and IScopeAccessor
            builder.Services.AddSingleton<IScopeProvider>(f => f.GetRequiredService<ScopeProvider>());
            builder.Services.AddSingleton<IScopeAccessor>(f => f.GetRequiredService<ScopeProvider>());
            builder.Services.AddScoped<IHttpScopeReference, HttpScopeReference>();

            builder.Services.AddSingleton<IJsonSerializer, JsonNetSerializer>();
            builder.Services.AddSingleton<IConfigurationEditorJsonSerializer, ConfigurationEditorJsonSerializer>();
            builder.Services.AddSingleton<IMenuItemCollectionFactory, MenuItemCollectionFactory>();

            // register database builder
            // *not* a singleton, don't want to keep it around
            builder.Services.AddTransient<DatabaseBuilder>();

            // register manifest parser, will be injected in collection builders where needed
            builder.Services.AddSingleton<IManifestParser, ManifestParser>();

            // register the manifest filter collection builder (collection is empty by default)
            builder.ManifestFilters();

            builder.MediaUrlGenerators()
                .Add<FileUploadPropertyEditor>()
                .Add<ImageCropperPropertyEditor>();

            builder.Services.AddSingleton<IPublishedContentTypeFactory, PublishedContentTypeFactory>();

            builder.Services.AddSingleton<IShortStringHelper>(factory
                => new DefaultShortStringHelper(new DefaultShortStringHelperConfig().WithDefault(factory.GetRequiredService<IOptions<RequestHandlerSettings>>().Value)));

            builder.Services.AddSingleton<IMigrationPlanExecutor, MigrationPlanExecutor>();
            builder.Services.AddSingleton<IMigrationBuilder>(factory => new MigrationBuilder(factory));

            builder.AddPreValueMigrators();

            builder.Services.AddSingleton<IPublishedSnapshotRebuilder, PublishedSnapshotRebuilder>();

            // register the published snapshot accessor - the "current" published snapshot is in the sbn context
            builder.Services.AddSingleton<IPublishedSnapshotAccessor, SbnContextPublishedSnapshotAccessor>();

            builder.Services.AddSingleton<IVariationContextAccessor, HybridVariationContextAccessor>();

            // Config manipulator
            builder.Services.AddSingleton<IConfigManipulator, JsonConfigManipulator>();

            builder.Services.AddSingleton<RichTextEditorPastedImages>();
            builder.Services.AddSingleton<BlockEditorConverter>();

            // both TinyMceValueConverter (in Core) and RteMacroRenderingValueConverter (in Web) will be
            // discovered when CoreBootManager configures the converters. We will remove the basic one defined
            // in core so that the more enhanced version is active.
            builder.PropertyValueConverters()
                .Remove<SimpleTinyMceValueConverter>();

            // register *all* checks, except those marked [HideFromTypeFinder] of course
            builder.Services.AddSingleton<IMarkdownToHtmlConverter, MarkdownToHtmlConverter>();

            builder.Services.AddSingleton<IContentLastChanceFinder, ContentFinderByConfigured404>();

            builder.Services.AddScoped<SbnTreeSearcher>();

            // replace
            builder.Services.AddSingleton<IEmailSender, EmailSender>(
                services => new EmailSender(
                    services.GetRequiredService<ILogger<EmailSender>>(),
                    services.GetRequiredService<IOptions<GlobalSettings>>(),
                    services.GetRequiredService<IEventAggregator>(),
                    services.GetService<INotificationHandler<SendEmailNotification>>(),
                    services.GetService<INotificationAsyncHandler<SendEmailNotification>>()));

            builder.Services.AddSingleton<IExamineManager, ExamineManager>();

            builder.Services.AddScoped<ITagQuery, TagQuery>();

            builder.Services.AddSingleton<ISbnTreeSearcherFields, SbnTreeSearcherFields>();
            builder.Services.AddSingleton<IPublishedContentQueryAccessor, PublishedContentQueryAccessor>();
            builder.Services.AddScoped<IPublishedContentQuery>(factory =>
            {
                var umbCtx = factory.GetRequiredService<ISbnContextAccessor>();
                var sbnContext = umbCtx.GetRequiredSbnContext();
                return new PublishedContentQuery(sbnContext.PublishedSnapshot, factory.GetRequiredService<IVariationContextAccessor>(), factory.GetRequiredService<IExamineManager>());
            });

            // register accessors for cultures
            builder.Services.AddSingleton<IDefaultCultureAccessor, DefaultCultureAccessor>();

            builder.Services.AddSingleton<IFilePermissionHelper, FilePermissionHelper>();

            builder.Services.AddSingleton<ISbnComponentRenderer, SbnComponentRenderer>();

            builder.Services.AddSingleton<IBackOfficeExamineSearcher, NoopBackOfficeExamineSearcher>();

            builder.Services.AddSingleton<UploadAutoFillProperties>();

            builder.Services.AddSingleton<ICronTabParser, NCronTabParser>();

            // Add default ImageSharp configuration and service implementations
            builder.Services.AddSingleton(SixLabors.ImageSharp.Configuration.Default);
            builder.Services.AddSingleton<IImageDimensionExtractor, ImageSharpDimensionExtractor>();
            builder.Services.AddSingleton<IImageUrlGenerator, ImageSharpImageUrlGenerator>();

            builder.Services.AddSingleton<PackageDataInstallation>();

            builder.AddInstaller();

            // Services required to run background jobs (with out the handler)
            builder.Services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();

            return builder;
        }

        /// <summary>
        /// Adds logging requirements for Sbn
        /// </summary>
        private static ISbnBuilder AddLogging(this ISbnBuilder builder)
        {
            builder.Services.AddSingleton<ThreadAbortExceptionEnricher>();
            builder.Services.AddSingleton<HttpSessionIdEnricher>();
            builder.Services.AddSingleton<HttpRequestNumberEnricher>();
            builder.Services.AddSingleton<HttpRequestIdEnricher>();
            return builder;
        }

        private static ISbnBuilder AddMainDom(this ISbnBuilder builder)
        {
            builder.Services.AddSingleton<IMainDomLock>(factory =>
            {
                var globalSettings = factory.GetRequiredService<IOptions<GlobalSettings>>();
                var connectionStrings = factory.GetRequiredService<IOptionsMonitor<ConnectionStrings>>();
                var hostingEnvironment = factory.GetRequiredService<IHostingEnvironment>();

                var dbCreator = factory.GetRequiredService<IDbProviderFactoryCreator>();
                var databaseSchemaCreatorFactory = factory.GetRequiredService<DatabaseSchemaCreatorFactory>();
                var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
                var loggerFactory = factory.GetRequiredService<ILoggerFactory>();
                var npocoMappers = factory.GetRequiredService<NPocoMapperCollection>();

                return globalSettings.Value.MainDomLock.Equals("SqlMainDomLock") || isWindows == false
                    ? (IMainDomLock)new SqlMainDomLock(
                            loggerFactory.CreateLogger<SqlMainDomLock>(),
                            loggerFactory,
                            globalSettings,
                            connectionStrings,
                            dbCreator,
                            hostingEnvironment,
                            databaseSchemaCreatorFactory,
                            npocoMappers)
                    : new MainDomSemaphoreLock(loggerFactory.CreateLogger<MainDomSemaphoreLock>(), hostingEnvironment);
            });

            return builder;
        }


        private static ISbnBuilder AddPreValueMigrators(this ISbnBuilder builder)
        {
            builder.WithCollectionBuilder<PreValueMigratorCollectionBuilder>()
                .Append<RenamingPreValueMigrator>()
                .Append<RichTextPreValueMigrator>()
                .Append<SbnSliderPreValueMigrator>()
                .Append<MediaPickerPreValueMigrator>()
                .Append<ContentPickerPreValueMigrator>()
                .Append<NestedContentPreValueMigrator>()
                .Append<DecimalPreValueMigrator>()
                .Append<ListViewPreValueMigrator>()
                .Append<DropDownFlexiblePreValueMigrator>()
                .Append<ValueListPreValueMigrator>()
                .Append<MarkdownEditorPreValueMigrator>();

            return builder;
        }

        public static ISbnBuilder AddLogViewer(this ISbnBuilder builder)
        {
            builder.Services.AddSingleton<ILogViewerConfig, LogViewerConfig>();
            builder.SetLogViewer<SerilogJsonLogViewer>();
            builder.Services.AddSingleton<ILogViewer>(factory => new SerilogJsonLogViewer(factory.GetRequiredService<ILogger<SerilogJsonLogViewer>>(),
                factory.GetRequiredService<ILogViewerConfig>(),
                factory.GetRequiredService<ILoggingConfiguration>(),
                Log.Logger));

            return builder;
        }


        public static ISbnBuilder AddCoreNotifications(this ISbnBuilder builder)
        {
            // add handlers for sending user notifications (i.e. emails)
            builder.Services.AddSingleton<UserNotificationsHandler.Notifier>();
            builder
                .AddNotificationHandler<ContentSavedNotification, UserNotificationsHandler>()
                .AddNotificationHandler<ContentSortedNotification, UserNotificationsHandler>()
                .AddNotificationHandler<ContentPublishedNotification, UserNotificationsHandler>()
                .AddNotificationHandler<ContentMovedNotification, UserNotificationsHandler>()
                .AddNotificationHandler<ContentMovedToRecycleBinNotification, UserNotificationsHandler>()
                .AddNotificationHandler<ContentCopiedNotification, UserNotificationsHandler>()
                .AddNotificationHandler<ContentRolledBackNotification, UserNotificationsHandler>()
                .AddNotificationHandler<ContentSentToPublishNotification, UserNotificationsHandler>()
                .AddNotificationHandler<ContentUnpublishedNotification, UserNotificationsHandler>()
                .AddNotificationHandler<AssignedUserGroupPermissionsNotification, UserNotificationsHandler>()
                .AddNotificationHandler<PublicAccessEntrySavedNotification, UserNotificationsHandler>();

            // add handlers for building content relations
            builder
                .AddNotificationHandler<ContentCopiedNotification, RelateOnCopyNotificationHandler>()
                .AddNotificationHandler<ContentMovedNotification, RelateOnTrashNotificationHandler>()
                .AddNotificationHandler<ContentMovedToRecycleBinNotification, RelateOnTrashNotificationHandler>()
                .AddNotificationHandler<MediaMovedNotification, RelateOnTrashNotificationHandler>()
                .AddNotificationHandler<MediaMovedToRecycleBinNotification, RelateOnTrashNotificationHandler>();

            // add notification handlers for property editors
            builder
                .AddNotificationHandler<ContentSavingNotification, BlockEditorPropertyHandler>()
                .AddNotificationHandler<ContentCopyingNotification, BlockEditorPropertyHandler>()
                .AddNotificationHandler<ContentSavingNotification, NestedContentPropertyHandler>()
                .AddNotificationHandler<ContentCopyingNotification, NestedContentPropertyHandler>()
                .AddNotificationHandler<ContentCopiedNotification, FileUploadPropertyEditor>()
                .AddNotificationHandler<ContentDeletedNotification, FileUploadPropertyEditor>()
                .AddNotificationHandler<MediaDeletedNotification, FileUploadPropertyEditor>()
                .AddNotificationHandler<MediaSavingNotification, FileUploadPropertyEditor>()
                .AddNotificationHandler<MemberDeletedNotification, FileUploadPropertyEditor>()
                .AddNotificationHandler<ContentCopiedNotification, ImageCropperPropertyEditor>()
                .AddNotificationHandler<ContentDeletedNotification, ImageCropperPropertyEditor>()
                .AddNotificationHandler<MediaDeletedNotification, ImageCropperPropertyEditor>()
                .AddNotificationHandler<MediaSavingNotification, ImageCropperPropertyEditor>()
                .AddNotificationHandler<MemberDeletedNotification, ImageCropperPropertyEditor>();

            // add notification handlers for redirect tracking
            builder
                .AddNotificationHandler<ContentPublishingNotification, RedirectTrackingHandler>()
                .AddNotificationHandler<ContentPublishedNotification, RedirectTrackingHandler>()
                .AddNotificationHandler<ContentMovingNotification, RedirectTrackingHandler>()
                .AddNotificationHandler<ContentMovedNotification, RedirectTrackingHandler>();

            // Add notification handlers for DistributedCache
            builder
                .AddNotificationHandler<DictionaryItemDeletedNotification, DistributedCacheBinder>()
                .AddNotificationHandler<DictionaryItemSavedNotification, DistributedCacheBinder>()
                .AddNotificationHandler<LanguageSavedNotification, DistributedCacheBinder>()
                .AddNotificationHandler<LanguageDeletedNotification, DistributedCacheBinder>()
                .AddNotificationHandler<MemberSavedNotification, DistributedCacheBinder>()
                .AddNotificationHandler<MemberDeletedNotification, DistributedCacheBinder>()
                .AddNotificationHandler<PublicAccessEntrySavedNotification, DistributedCacheBinder>()
                .AddNotificationHandler<PublicAccessEntryDeletedNotification, DistributedCacheBinder>()
                .AddNotificationHandler<UserSavedNotification, DistributedCacheBinder>()
                .AddNotificationHandler<UserDeletedNotification, DistributedCacheBinder>()
                .AddNotificationHandler<UserGroupWithUsersSavedNotification, DistributedCacheBinder>()
                .AddNotificationHandler<UserGroupDeletedNotification, DistributedCacheBinder>()
                .AddNotificationHandler<MemberGroupDeletedNotification, DistributedCacheBinder>()
                .AddNotificationHandler<MemberGroupSavedNotification, DistributedCacheBinder>()
                .AddNotificationHandler<DataTypeDeletedNotification, DistributedCacheBinder>()
                .AddNotificationHandler<DataTypeSavedNotification, DistributedCacheBinder>()
                .AddNotificationHandler<TemplateDeletedNotification, DistributedCacheBinder>()
                .AddNotificationHandler<TemplateSavedNotification, DistributedCacheBinder>()
                .AddNotificationHandler<RelationTypeDeletedNotification, DistributedCacheBinder>()
                .AddNotificationHandler<RelationTypeSavedNotification, DistributedCacheBinder>()
                .AddNotificationHandler<DomainDeletedNotification, DistributedCacheBinder>()
                .AddNotificationHandler<DomainSavedNotification, DistributedCacheBinder>()
                .AddNotificationHandler<MacroSavedNotification, DistributedCacheBinder>()
                .AddNotificationHandler<MacroDeletedNotification, DistributedCacheBinder>()
                .AddNotificationHandler<MediaTreeChangeNotification, DistributedCacheBinder>()
                .AddNotificationHandler<ContentTypeChangedNotification, DistributedCacheBinder>()
                .AddNotificationHandler<MediaTypeChangedNotification, DistributedCacheBinder>()
                .AddNotificationHandler<MemberTypeChangedNotification, DistributedCacheBinder>()
                .AddNotificationHandler<ContentTreeChangeNotification, DistributedCacheBinder>()
                ;
            // add notification handlers for auditing
            builder
                .AddNotificationHandler<MemberSavedNotification, AuditNotificationsHandler>()
                .AddNotificationHandler<MemberDeletedNotification, AuditNotificationsHandler>()
                .AddNotificationHandler<AssignedMemberRolesNotification, AuditNotificationsHandler>()
                .AddNotificationHandler<RemovedMemberRolesNotification, AuditNotificationsHandler>()
                .AddNotificationHandler<ExportedMemberNotification, AuditNotificationsHandler>()
                .AddNotificationHandler<UserSavedNotification, AuditNotificationsHandler>()
                .AddNotificationHandler<UserDeletedNotification, AuditNotificationsHandler>()
                .AddNotificationHandler<UserGroupWithUsersSavedNotification, AuditNotificationsHandler>()
                .AddNotificationHandler<AssignedUserGroupPermissionsNotification, AuditNotificationsHandler>();

            return builder;
        }
    }
}
