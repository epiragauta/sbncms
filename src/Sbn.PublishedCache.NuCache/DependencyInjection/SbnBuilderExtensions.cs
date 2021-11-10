using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.DependencyInjection;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Notifications;
using Sbn.Cms.Core.PropertyEditors;
using Sbn.Cms.Core.PublishedCache;
using Sbn.Cms.Core.Scoping;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Infrastructure.PublishedCache;
using Sbn.Cms.Infrastructure.PublishedCache.DataSource;
using Sbn.Cms.Infrastructure.PublishedCache.Persistence;

namespace Sbn.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="ISbnBuilder"/> for the Sbn's NuCache
    /// </summary>
    public static class SbnBuilderExtensions
    {
        /// <summary>
        /// Adds Sbn NuCache dependencies
        /// </summary>
        public static ISbnBuilder AddNuCache(this ISbnBuilder builder)
        {
            // register the NuCache database data source
            builder.Services.TryAddSingleton<INuCacheContentRepository, NuCacheContentRepository>();
            builder.Services.TryAddSingleton<INuCacheContentService, NuCacheContentService>();
            builder.Services.TryAddSingleton<PublishedSnapshotServiceEventHandler>();

            // register the NuCache published snapshot service
            // must register default options, required in the service ctor
            builder.Services.TryAddTransient(factory => new PublishedSnapshotServiceOptions());
            builder.SetPublishedSnapshotService<PublishedSnapshotService>();
            builder.Services.TryAddSingleton<IPublishedSnapshotStatus, PublishedSnapshotStatus>();

            // replace this service since we want to improve the content/media
            // mapping lookups if we are using nucache.
            // TODO: Gotta wonder how much this does actually improve perf? It's a lot of weird code to make this happen so hope it's worth it
            builder.Services.AddUnique<IIdKeyMap>(factory =>
            {
                var idkSvc = new IdKeyMap(factory.GetRequiredService<IScopeProvider>());
                if (factory.GetRequiredService<IPublishedSnapshotService>() is PublishedSnapshotService publishedSnapshotService)
                {
                    idkSvc.SetMapper(SbnObjectTypes.Document, id => publishedSnapshotService.GetDocumentUid(id), uid => publishedSnapshotService.GetDocumentId(uid));
                    idkSvc.SetMapper(SbnObjectTypes.Media, id => publishedSnapshotService.GetMediaUid(id), uid => publishedSnapshotService.GetMediaId(uid));
                }

                return idkSvc;
            });

            builder.AddNuCacheNotifications();

            builder.AddNotificationHandler<SbnApplicationStartingNotification, NuCacheStartupHandler>();
            builder.Services.AddSingleton<IContentCacheDataSerializerFactory>(s =>
            {
                IOptions<NuCacheSettings> options = s.GetRequiredService<IOptions<NuCacheSettings>>();
                switch (options.Value.NuCacheSerializerType)
                {
                    case NuCacheSerializerType.JSON:
                        return new JsonContentNestedDataSerializerFactory();
                    case NuCacheSerializerType.MessagePack:
                        return ActivatorUtilities.CreateInstance<MsgPackContentNestedDataSerializerFactory>(s);
                    default:
                        throw new IndexOutOfRangeException();
                }
            });

            builder.Services.AddSingleton<IPropertyCacheCompressionOptions>(s =>
            {
                IOptions<NuCacheSettings> options = s.GetRequiredService<IOptions<NuCacheSettings>>();

                if (options.Value.NuCacheSerializerType == NuCacheSerializerType.MessagePack &&
                    options.Value.UnPublishedContentCompression)
                {
                    return new UnPublishedContentPropertyCacheCompressionOptions();
                }

                return new NoopPropertyCacheCompressionOptions();
            });

            builder.Services.AddSingleton(s => new ContentDataSerializer(new DictionaryOfPropertyDataSerializer()));

            // add the NuCache health check (hidden from type finder)
            // TODO: no NuCache health check yet
            // composition.HealthChecks().Add<NuCacheIntegrityHealthCheck>();
            return builder;
        }


        private static ISbnBuilder AddNuCacheNotifications(this ISbnBuilder builder)
        {
            builder
                .AddNotificationHandler<LanguageSavedNotification, PublishedSnapshotServiceEventHandler>()
                .AddNotificationHandler<MemberDeletingNotification, PublishedSnapshotServiceEventHandler>()
#pragma warning disable CS0618 // Type or member is obsolete
                .AddNotificationHandler<ContentRefreshNotification, PublishedSnapshotServiceEventHandler>()
                .AddNotificationHandler<MediaRefreshNotification, PublishedSnapshotServiceEventHandler>()
                .AddNotificationHandler<MemberRefreshNotification, PublishedSnapshotServiceEventHandler>()
                .AddNotificationHandler<ContentTypeRefreshedNotification, PublishedSnapshotServiceEventHandler>()
                .AddNotificationHandler<MediaTypeRefreshedNotification, PublishedSnapshotServiceEventHandler>()
                .AddNotificationHandler<MemberTypeRefreshedNotification, PublishedSnapshotServiceEventHandler>()
                .AddNotificationHandler<ScopedEntityRemoveNotification, PublishedSnapshotServiceEventHandler>()
#pragma warning restore CS0618 // Type or member is obsolete
                ;

            return builder;
        }


    }
}
