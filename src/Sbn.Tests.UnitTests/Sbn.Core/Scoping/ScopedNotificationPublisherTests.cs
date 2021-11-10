using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Sbn.Cms.Core.Cache;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Hosting;
using Sbn.Cms.Core.IO;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Notifications;
using Sbn.Cms.Core.PropertyEditors;
using Sbn.Cms.Core.Scoping;
using Sbn.Cms.Core.Strings;
using Sbn.Cms.Infrastructure.Persistence;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Core.Scoping
{
    [TestFixture]
    public class ScopedNotificationPublisherTests
    {

        [Test]
        public void ScopeUsesInjectedNotificationPublisher()
        {
            var notificationPublisherMock = new Mock<IScopedNotificationPublisher>();
            ScopeProvider scopeProvider = GetScopeProvider(out var eventAggregatorMock);

            using (IScope scope = scopeProvider.CreateScope(notificationPublisher: notificationPublisherMock.Object))
            {
                scope.Notifications.Publish(Mock.Of<INotification>());
                scope.Notifications.PublishCancelable(Mock.Of<ICancelableNotification>());

                notificationPublisherMock.Verify(x => x.Publish(It.IsAny<INotification>()), Times.Once);
                notificationPublisherMock.Verify(x => x.PublishCancelable(It.IsAny<ICancelableNotification>()), Times.Once);

                // Ensure that the custom scope provider is till used in inner scope.
                using (IScope innerScope = scopeProvider.CreateScope())
                {
                    innerScope.Notifications.Publish(Mock.Of<INotification>());
                    innerScope.Notifications.PublishCancelable(Mock.Of<ICancelableNotification>());

                    notificationPublisherMock.Verify(x => x.Publish(It.IsAny<INotification>()), Times.Exactly(2));
                    notificationPublisherMock.Verify(x => x.PublishCancelable(It.IsAny<ICancelableNotification>()), Times.Exactly(2));
                }

                // Ensure scope exit is not called until outermost scope is being disposed
                notificationPublisherMock.Verify(x => x.ScopeExit(It.IsAny<bool>()), Times.Never());
            }

            notificationPublisherMock.Verify(x => x.ScopeExit(It.IsAny<bool>()), Times.Once());
            // Ensure that the event aggregator isn't used directly.
            eventAggregatorMock.Verify(x => x.Publish(It.IsAny<INotification>()), Times.Never);
            eventAggregatorMock.Verify(x => x.PublishCancelable(It.IsAny<ICancelableNotification>()), Times.Never);
        }

        [Test]
        public void SpecifyingNotificationPublishInInnerScopeCausesError()
        {
            var notificationPublisherMock = new Mock<IScopedNotificationPublisher>();
            ScopeProvider scopeProvider = GetScopeProvider(out var eventAggregatorMock);

            using (var scope = scopeProvider.CreateScope())
            {
                Assert.Throws<ArgumentException>(() => scopeProvider.CreateScope(notificationPublisher: notificationPublisherMock.Object));
            }
        }

        private ScopeProvider GetScopeProvider(out Mock<IEventAggregator> eventAggregatorMock)
        {
            NullLoggerFactory loggerFactory = NullLoggerFactory.Instance;

            var fileSystems = new FileSystems(
                loggerFactory,
                Mock.Of<IIOHelper>(),
                Options.Create(new GlobalSettings()),
                Mock.Of<IHostingEnvironment>());

            var mediaFileManager = new MediaFileManager(
                Mock.Of<IFileSystem>(),
                Mock.Of<IMediaPathScheme>(),
                loggerFactory.CreateLogger<MediaFileManager>(),
                Mock.Of<IShortStringHelper>(),
                Mock.Of<IServiceProvider>(),
                Options.Create(new ContentSettings()));

            eventAggregatorMock = new Mock<IEventAggregator>();

            return new ScopeProvider(
                Mock.Of<ISbnDatabaseFactory>(),
                fileSystems,
                Options.Create(new CoreDebugSettings()),
                mediaFileManager,
                loggerFactory.CreateLogger<ScopeProvider>(),
                loggerFactory,
                Mock.Of<IRequestCache>(),
                eventAggregatorMock.Object
            );
        }
    }
}
