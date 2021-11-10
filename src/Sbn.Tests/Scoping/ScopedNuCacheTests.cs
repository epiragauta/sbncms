using System;
using System.Web.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework;
using Sbn.Cms.Core;
using Sbn.Cms.Core.Cache;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.DependencyInjection;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Hosting;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Models.PublishedContent;
using Sbn.Cms.Core.Notifications;
using Sbn.Cms.Core.Persistence.Repositories;
using Sbn.Cms.Core.PublishedCache;
using Sbn.Cms.Core.Security;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Strings;
using Sbn.Cms.Core.Sync;
using Sbn.Cms.Core.Web;
using Sbn.Cms.Infrastructure.PublishedCache;
using Sbn.Cms.Infrastructure.PublishedCache.Persistence;
using Sbn.Cms.Tests.Common;
using Sbn.Cms.Tests.Common.Testing;
using Sbn.Extensions;
using Sbn.Tests.TestHelpers;
using Sbn.Web;
using Sbn.Web.Composing;

namespace Sbn.Tests.Scoping
{
    [TestFixture]
    [SbnTest(Database = SbnTestOptions.Database.NewSchemaPerTest, PublishedRepositoryEvents = true)]
    public class ScopedNuCacheTests : TestWithDatabaseBase
    {
        private DistributedCacheBinder _distributedCacheBinder;

        protected override void Compose()
        {
            base.Compose();

            // the cache refresher component needs to trigger to refresh caches
            // but then, it requires a lot of plumbing ;(
            // FIXME: and we cannot inject a DistributedCache yet
            // so doing all this mess
            Builder.Services.AddUnique<IServerMessenger, ScopedXmlTests.LocalServerMessenger>();
            Builder.Services.AddUnique(f => Mock.Of<IServerRoleAccessor>());
            Builder.WithCollectionBuilder<CacheRefresherCollectionBuilder>()
                .Add(() => Builder.TypeLoader.GetCacheRefreshers());
            Builder.AddNotificationHandler<ContentPublishedNotification, NotificationHandler>();
        }

        public class NotificationHandler : INotificationHandler<ContentPublishedNotification>
        {
            public void Handle(ContentPublishedNotification notification) => PublishedContent?.Invoke(notification);

            public static Action<ContentPublishedNotification> PublishedContent { get; set; }
        }

        public override void TearDown()
        {
            base.TearDown();

            NotificationHandler.PublishedContent = null;
        }

        protected override IPublishedSnapshotService CreatePublishedSnapshotService(GlobalSettings globalSettings = null)
        {
            var options = new PublishedSnapshotServiceOptions { IgnoreLocalDb = true };
            var publishedSnapshotAccessor = new SbnContextPublishedSnapshotAccessor(Current.SbnContextAccessor);
            var runtimeStateMock = new Mock<IRuntimeState>();
            runtimeStateMock.Setup(x => x.Level).Returns(() => RuntimeLevel.Run);

            var contentTypeFactory = Factory.GetRequiredService<IPublishedContentTypeFactory>();
            var documentRepository = Mock.Of<IDocumentRepository>();
            var mediaRepository = Mock.Of<IMediaRepository>();
            var memberRepository = Mock.Of<IMemberRepository>();
            var hostingEnvironment = TestHelper.GetHostingEnvironment();

            var typeFinder = TestHelper.GetTypeFinder();

            var nuCacheSettings = new NuCacheSettings();
            var lifetime = new Mock<ISbnApplicationLifetime>();
            var repository = new NuCacheContentRepository(ScopeProvider, AppCaches.Disabled, Mock.Of<ILogger<NuCacheContentRepository>>(), memberRepository, documentRepository, mediaRepository, Mock.Of<IShortStringHelper>(), new UrlSegmentProviderCollection(new[] { new DefaultUrlSegmentProvider(ShortStringHelper) }));
            var snapshotService = new PublishedSnapshotService(
                options,
                null,
                ServiceContext,
                contentTypeFactory,
                publishedSnapshotAccessor,
                Mock.Of<IVariationContextAccessor>(),
                base.ProfilingLogger,
                NullLoggerFactory.Instance,
                ScopeProvider,
                new NuCacheContentService(repository, ScopeProvider, NullLoggerFactory.Instance, Mock.Of<IEventMessagesFactory>()),
                DefaultCultureAccessor,
                Microsoft.Extensions.Options.Options.Create(globalSettings ?? new GlobalSettings()),
                Factory.GetRequiredService<IEntityXmlSerializer>(),
                new NoopPublishedModelFactory(),
                hostingEnvironment,
                Microsoft.Extensions.Options.Options.Create(nuCacheSettings));

            return snapshotService;
        }

        protected ISbnContext GetSbnContextNu(string url, RouteData routeData = null, bool setSingleton = false)
        {
            // ensure we have a PublishedSnapshotService
            var service = PublishedSnapshotService as PublishedSnapshotService;

            var httpContext = GetHttpContextFactory(url, routeData).HttpContext;
            var httpContextAccessor = TestHelper.GetHttpContextAccessor(httpContext);
            var globalSettings = TestObjects.GetGlobalSettings();
            var sbnContext = new SbnContext(
                httpContextAccessor,
                service,
                Mock.Of<IBackOfficeSecurity>(),
                globalSettings,
                HostingEnvironment,
                new TestVariationContextAccessor(),
                UriUtility,
                new AspNetCookieManager(httpContextAccessor));

            if (setSingleton)
                Sbn.Web.Composing.Current.SbnContextAccessor.SbnContext = sbnContext;

            return sbnContext;
        }

        [TestCase(true)]
        [TestCase(false)]
        public void TestScope(bool complete)
        {
            var sbnContext = GetSbnContextNu("http://example.com/", setSingleton: true);

            // wire cache refresher
            _distributedCacheBinder = new DistributedCacheBinder(new DistributedCache(Current.ServerMessenger, Current.CacheRefreshers));

            // create document type, document
            var contentType = new ContentType(ShortStringHelper, -1) { Alias = "CustomDocument", Name = "Custom Document" };
            ServiceContext.ContentTypeService.Save(contentType);
            var item = new Content("name", -1, contentType);

            // event handler
            var evented = 0;
            NotificationHandler.PublishedContent = notification =>
            {
                evented++;

                var e = sbnContext.Content.GetById(item.Id);

                // during events, due to LiveSnapshot, we see the changes
                Assert.IsNotNull(e);
                Assert.AreEqual("changed", e.Name(VariationContextAccessor));
            };

            using (var scope = ScopeProvider.CreateScope())
            {
                ServiceContext.ContentService.SaveAndPublish(item);
                scope.Complete();
            }

            // been created
            var x = sbnContext.Content.GetById(item.Id);
            Assert.IsNotNull(x);
            Assert.AreEqual("name", x.Name(VariationContextAccessor));

            using (var scope = ScopeProvider.CreateScope())
            {
                item.Name = "changed";
                ServiceContext.ContentService.SaveAndPublish(item);

                if (complete)
                    scope.Complete();
            }

            // only 1 event occuring because we are publishing twice for the same event for
            // the same object and the scope deduplicates the events (uses the latest)
            Assert.AreEqual(complete ? 1 : 0, evented);

            // after the scope,
            // if completed, we see the changes
            // else changes have been rolled back
            x = sbnContext.Content.GetById(item.Id);
            Assert.IsNotNull(x);
            Assert.AreEqual(complete ? "changed" : "name", x.Name(VariationContextAccessor));
        }
    }
}
