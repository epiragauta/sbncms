using Sbn.Cms.Core.Actions;
using Sbn.Cms.Core.Cache;
using Sbn.Cms.Core.Composing;
using Sbn.Cms.Core.ContentApps;
using Sbn.Cms.Core.Dashboards;
using Sbn.Cms.Core.Editors;
using Sbn.Cms.Core.HealthChecks;
using Sbn.Cms.Core.HealthChecks.NotificationMethods;
using Sbn.Cms.Core.Manifest;
using Sbn.Cms.Core.Mapping;
using Sbn.Cms.Core.Media.EmbedProviders;
using Sbn.Cms.Core.Packaging;
using Sbn.Cms.Core.PropertyEditors;
using Sbn.Cms.Core.PropertyEditors.Validators;
using Sbn.Cms.Core.Routing;
using Sbn.Cms.Core.Sections;
using Sbn.Cms.Core.Strings;
using Sbn.Cms.Core.Tour;
using Sbn.Cms.Core.Trees;
using Sbn.Cms.Core.WebAssets;
using Sbn.Extensions;

namespace Sbn.Cms.Core.DependencyInjection
{
    /// <summary>
    /// Extension methods for <see cref="ISbnBuilder"/>
    /// </summary>
    public static partial class SbnBuilderExtensions
    {
        /// <summary>
        /// Adds all core collection builders
        /// </summary>
        internal static void AddAllCoreCollectionBuilders(this ISbnBuilder builder)
        {
            builder.CacheRefreshers().Add(() => builder.TypeLoader.GetCacheRefreshers());
            builder.DataEditors().Add(() => builder.TypeLoader.GetDataEditors());
            builder.Actions().Add(() => builder.TypeLoader.GetActions());

            // register known content apps
            builder.ContentApps()
                .Append<ListViewContentAppFactory>()
                .Append<ContentEditorContentAppFactory>()
                .Append<ContentInfoContentAppFactory>()
                .Append<ContentTypeDesignContentAppFactory>()
                .Append<ContentTypeListViewContentAppFactory>()
                .Append<ContentTypePermissionsContentAppFactory>()
                .Append<ContentTypeTemplatesContentAppFactory>();

            // all built-in finders in the correct order,
            // devs can then modify this list on application startup
            builder.ContentFinders()
                .Append<ContentFinderByPageIdQuery>()
                .Append<ContentFinderByUrl>()
                .Append<ContentFinderByIdPath>()
                /*.Append<ContentFinderByUrlAndTemplate>() // disabled, this is an odd finder */
                .Append<ContentFinderByUrlAlias>()
                .Append<ContentFinderByRedirectUrl>();
            builder.EditorValidators().Add(() => builder.TypeLoader.GetTypes<IEditorValidator>());
            builder.HealthChecks().Add(() => builder.TypeLoader.GetTypes<HealthCheck>());
            builder.HealthCheckNotificationMethods().Add(() => builder.TypeLoader.GetTypes<IHealthCheckNotificationMethod>());
            builder.TourFilters();
            builder.UrlProviders()
                .Append<AliasUrlProvider>()
                .Append<DefaultUrlProvider>();
            builder.MediaUrlProviders()
                .Append<DefaultMediaUrlProvider>();
            // register back office sections in the order we want them rendered
            builder.Sections()
                .Append<ContentSection>()
                .Append<MediaSection>()
                .Append<SettingsSection>()
                .Append<PackagesSection>()
                .Append<UsersSection>()
                .Append<MembersSection>()
                .Append<FormsSection>()
                .Append<TranslationSection>();
            builder.Components();
            // register core CMS dashboards and 3rd party types - will be ordered by weight attribute & merged with package.manifest dashboards
            builder.Dashboards()
                .Add<ContentDashboard>()
                .Add<ExamineDashboard>()
                .Add<FormsDashboard>()
                .Add<HealthCheckDashboard>()
                .Add<ManifestDashboard>()
                .Add<MediaDashboard>()
                .Add<MembersDashboard>()
                .Add<ProfilerDashboard>()
                .Add<PublishedStatusDashboard>()
                .Add<RedirectUrlDashboard>()
                .Add<SettingsDashboard>()
                .Add(builder.TypeLoader.GetTypes<IDashboard>());
            builder.DataValueReferenceFactories();
            builder.PropertyValueConverters().Append(builder.TypeLoader.GetTypes<IPropertyValueConverter>());
            builder.UrlSegmentProviders().Append<DefaultUrlSegmentProvider>();
            builder.ManifestValueValidators()
                .Add<RequiredValidator>()
                .Add<RegexValidator>()
                .Add<DelimitedValueValidator>()
                .Add<EmailValidator>()
                .Add<IntegerValidator>()
                .Add<DecimalValidator>();
            builder.ManifestFilters();
            builder.MediaUrlGenerators();
            // register OEmbed providers - no type scanning - all explicit opt-in of adding types, IEmbedProvider is not IDiscoverable
            builder.OEmbedProviders()
                .Append<YouTube>()
                .Append<Twitter>()
                .Append<Vimeo>()
                .Append<DailyMotion>()
                .Append<Flickr>()
                .Append<Slideshare>()
                .Append<Kickstarter>()
                .Append<GettyImages>()
                .Append<Ted>()
                .Append<Soundcloud>()
                .Append<Issuu>()
                .Append<Hulu>()
                .Append<Giphy>();
            builder.SearchableTrees().Add(() => builder.TypeLoader.GetTypes<ISearchableTree>());
            builder.BackOfficeAssets();
        }

        /// <summary>
        /// Gets the actions collection builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static ActionCollectionBuilder Actions(this ISbnBuilder builder)
            => builder.WithCollectionBuilder<ActionCollectionBuilder>();

        /// <summary>
        /// Gets the content apps collection builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static ContentAppFactoryCollectionBuilder ContentApps(this ISbnBuilder builder)
            => builder.WithCollectionBuilder<ContentAppFactoryCollectionBuilder>();

        /// <summary>
        /// Gets the content finders collection builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static ContentFinderCollectionBuilder ContentFinders(this ISbnBuilder builder)
            => builder.WithCollectionBuilder<ContentFinderCollectionBuilder>();

        /// <summary>
        /// Gets the editor validators collection builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static EditorValidatorCollectionBuilder EditorValidators(this ISbnBuilder builder)
            => builder.WithCollectionBuilder<EditorValidatorCollectionBuilder>();

        /// <summary>
        /// Gets the health checks collection builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static HealthCheckCollectionBuilder HealthChecks(this ISbnBuilder builder)
            => builder.WithCollectionBuilder<HealthCheckCollectionBuilder>();

        public static HealthCheckNotificationMethodCollectionBuilder HealthCheckNotificationMethods(this ISbnBuilder builder)
            => builder.WithCollectionBuilder<HealthCheckNotificationMethodCollectionBuilder>();

        /// <summary>
        /// Gets the TourFilters collection builder.
        /// </summary>
        public static TourFilterCollectionBuilder TourFilters(this ISbnBuilder builder)
            => builder.WithCollectionBuilder<TourFilterCollectionBuilder>();

        /// <summary>
        /// Gets the URL providers collection builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static UrlProviderCollectionBuilder UrlProviders(this ISbnBuilder builder)
            => builder.WithCollectionBuilder<UrlProviderCollectionBuilder>();

        /// <summary>
        /// Gets the media url providers collection builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static MediaUrlProviderCollectionBuilder MediaUrlProviders(this ISbnBuilder builder)
            => builder.WithCollectionBuilder<MediaUrlProviderCollectionBuilder>();

        /// <summary>
        /// Gets the backoffice sections/applications collection builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static SectionCollectionBuilder Sections(this ISbnBuilder builder)
            => builder.WithCollectionBuilder<SectionCollectionBuilder>();

        /// <summary>
        /// Gets the components collection builder.
        /// </summary>
        public static ComponentCollectionBuilder Components(this ISbnBuilder builder)
            => builder.WithCollectionBuilder<ComponentCollectionBuilder>();

        /// <summary>
        /// Gets the backoffice dashboards collection builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static DashboardCollectionBuilder Dashboards(this ISbnBuilder builder)
            => builder.WithCollectionBuilder<DashboardCollectionBuilder>();

        /// <summary>
        /// Gets the cache refreshers collection builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static CacheRefresherCollectionBuilder CacheRefreshers(this ISbnBuilder builder)
            => builder.WithCollectionBuilder<CacheRefresherCollectionBuilder>();

        /// <summary>
        /// Gets the map definitions collection builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static MapDefinitionCollectionBuilder MapDefinitions(this ISbnBuilder builder)
            => builder.WithCollectionBuilder<MapDefinitionCollectionBuilder>();

        /// <summary>
        /// Gets the data editor collection builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static DataEditorCollectionBuilder DataEditors(this ISbnBuilder builder)
            => builder.WithCollectionBuilder<DataEditorCollectionBuilder>();

        /// <summary>
        /// Gets the data value reference factory collection builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static DataValueReferenceFactoryCollectionBuilder DataValueReferenceFactories(this ISbnBuilder builder)
            => builder.WithCollectionBuilder<DataValueReferenceFactoryCollectionBuilder>();

        /// <summary>
        /// Gets the property value converters collection builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static PropertyValueConverterCollectionBuilder PropertyValueConverters(this ISbnBuilder builder)
            => builder.WithCollectionBuilder<PropertyValueConverterCollectionBuilder>();

        /// <summary>
        /// Gets the url segment providers collection builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static UrlSegmentProviderCollectionBuilder UrlSegmentProviders(this ISbnBuilder builder)
            => builder.WithCollectionBuilder<UrlSegmentProviderCollectionBuilder>();

        /// <summary>
        /// Gets the validators collection builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        internal static ManifestValueValidatorCollectionBuilder ManifestValueValidators(this ISbnBuilder builder)
            => builder.WithCollectionBuilder<ManifestValueValidatorCollectionBuilder>();

        /// <summary>
        /// Gets the manifest filter collection builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static ManifestFilterCollectionBuilder ManifestFilters(this ISbnBuilder builder)
            => builder.WithCollectionBuilder<ManifestFilterCollectionBuilder>();

        /// <summary>
        /// Gets the content finders collection builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static MediaUrlGeneratorCollectionBuilder MediaUrlGenerators(this ISbnBuilder builder)
            => builder.WithCollectionBuilder<MediaUrlGeneratorCollectionBuilder>();

        /// <summary>
        /// Gets the backoffice OEmbed Providers collection builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static EmbedProvidersCollectionBuilder OEmbedProviders(this ISbnBuilder builder)
            => builder.WithCollectionBuilder<EmbedProvidersCollectionBuilder>();

        /// <summary>
        /// Gets the back office searchable tree collection builder
        /// </summary>
        public static SearchableTreeCollectionBuilder SearchableTrees(this ISbnBuilder builder)
            => builder.WithCollectionBuilder<SearchableTreeCollectionBuilder>();

        /// <summary>
        /// Gets the back office custom assets collection builder
        /// </summary>
        public static CustomBackOfficeAssetsCollectionBuilder BackOfficeAssets(this ISbnBuilder builder)
            => builder.WithCollectionBuilder<CustomBackOfficeAssetsCollectionBuilder>();
    }
}
