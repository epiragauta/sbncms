using Microsoft.Extensions.DependencyInjection;
using Sbn.Cms.Core.Cache;
using Sbn.Cms.Core.DependencyInjection;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Notifications;
using Sbn.Cms.Core.PropertyEditors;
using Sbn.Cms.Core.Scoping;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Strings;
using Sbn.Cms.Infrastructure.Examine;
using Sbn.Cms.Infrastructure.Search;
using Sbn.Extensions;

namespace Sbn.Cms.Infrastructure.DependencyInjection
{
    /// <summary>
    /// Provides extension methods to the <see cref="ISbnBuilder"/> class.
    /// </summary>
    public static partial class SbnBuilderExtensions
    {
        public static ISbnBuilder AddExamine(this ISbnBuilder builder)
        {
            // populators are not a collection: one cannot remove ours, and can only add more
            // the container can inject IEnumerable<IIndexPopulator> and get them all
            builder.Services.AddSingleton<IIndexPopulator, MemberIndexPopulator>();
            builder.Services.AddSingleton<IIndexPopulator, ContentIndexPopulator>();
            builder.Services.AddSingleton<IIndexPopulator, PublishedContentIndexPopulator>();
            builder.Services.AddSingleton<IIndexPopulator, MediaIndexPopulator>();

            builder.Services.AddSingleton<IIndexRebuilder, ExamineIndexRebuilder>();
            builder.Services.AddSingleton<ISbnIndexingHandler, ExamineSbnIndexingHandler>();
            builder.Services.AddUnique<ISbnIndexConfig, SbnIndexConfig>();
            builder.Services.AddUnique<IIndexDiagnosticsFactory, IndexDiagnosticsFactory>();
            builder.Services.AddUnique<IPublishedContentValueSetBuilder>(factory =>
                new ContentValueSetBuilder(
                    factory.GetRequiredService<PropertyEditorCollection>(),
                    factory.GetRequiredService<UrlSegmentProviderCollection>(),
                    factory.GetRequiredService<IUserService>(),
                    factory.GetRequiredService<IShortStringHelper>(),
                    factory.GetRequiredService<IScopeProvider>(),
                    true));
            builder.Services.AddUnique<IContentValueSetBuilder>(factory =>
                new ContentValueSetBuilder(
                    factory.GetRequiredService<PropertyEditorCollection>(),
                    factory.GetRequiredService<UrlSegmentProviderCollection>(),
                    factory.GetRequiredService<IUserService>(),
                    factory.GetRequiredService<IShortStringHelper>(),
                    factory.GetRequiredService<IScopeProvider>(),
                    false));
            builder.Services.AddUnique<IValueSetBuilder<IMedia>, MediaValueSetBuilder>();
            builder.Services.AddUnique<IValueSetBuilder<IMember>, MemberValueSetBuilder>();
            builder.Services.AddUnique<ExamineIndexRebuilder>();

            builder.AddNotificationHandler<ContentCacheRefresherNotification, ContentIndexingNotificationHandler>();
            builder.AddNotificationHandler<ContentTypeCacheRefresherNotification, ContentTypeIndexingNotificationHandler>();
            builder.AddNotificationHandler<MediaCacheRefresherNotification, MediaIndexingNotificationHandler>();
            builder.AddNotificationHandler<MemberCacheRefresherNotification, MemberIndexingNotificationHandler>();
            builder.AddNotificationHandler<LanguageCacheRefresherNotification, LanguageIndexingNotificationHandler>();

            builder.AddNotificationHandler<SbnRequestBeginNotification, RebuildOnStartupHandler>();

            return builder;
        }
    }
}
