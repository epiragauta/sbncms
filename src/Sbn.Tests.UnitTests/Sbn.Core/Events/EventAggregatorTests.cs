// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using Sbn.Cms.Core.DependencyInjection;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Notifications;
using Sbn.Cms.Tests.UnitTests.TestHelpers;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Core.Events
{
    [TestFixture]
    public class EventAggregatorTests
    {
        private const int A = 3;
        private const int B = 5;
        private const int C = 7;
        private ISbnBuilder _builder;

        [SetUp]
        public void Setup()
        {
            IServiceCollection register = TestHelper.GetServiceCollection();
            _builder = new SbnBuilder(register, Mock.Of<IConfiguration>(), TestHelper.GetMockedTypeLoader());
        }

        [Test]
        public async Task CanPublishAsyncEvents()
        {
            _builder.Services.AddScoped<Adder>();
            _builder.AddNotificationAsyncHandler<Notification, NotificationAsyncHandlerA>();
            _builder.AddNotificationAsyncHandler<Notification, NotificationAsyncHandlerB>();
            _builder.AddNotificationAsyncHandler<Notification, NotificationAsyncHandlerC>();
            ServiceProvider provider = _builder.Services.BuildServiceProvider();

            var notification = new Notification();
            IEventAggregator aggregator = provider.GetService<IEventAggregator>();
            await aggregator.PublishAsync(notification);

            Assert.AreEqual(A + B + C, notification.SubscriberCount);
        }

        [Test]
        public async Task CanPublishEvents()
        {
            _builder.Services.AddScoped<Adder>();
            _builder.AddNotificationHandler<Notification, NotificationHandlerA>();
            _builder.AddNotificationHandler<Notification, NotificationHandlerB>();
            _builder.AddNotificationHandler<Notification, NotificationHandlerC>();
            ServiceProvider provider = _builder.Services.BuildServiceProvider();

            var notification = new Notification();
            IEventAggregator aggregator = provider.GetService<IEventAggregator>();
            await aggregator.PublishAsync(notification);

            Assert.AreEqual(A + B + C, notification.SubscriberCount);
        }

        public class Notification : INotification
        {
            public int SubscriberCount { get; set; }
        }

        public class NotificationHandlerA : INotificationHandler<Notification>
        {
            public void Handle(Notification notification)
            {
                notification.SubscriberCount += A;
            }
        }

        public class NotificationHandlerB : INotificationHandler<Notification>
        {
            public void Handle(Notification notification)
            {
                notification.SubscriberCount += B;
            }
        }

        public class NotificationHandlerC : INotificationHandler<Notification>
        {
            private readonly Adder _adder;

            public NotificationHandlerC(Adder adder) => _adder = adder;

            public void Handle(Notification notification)
            {
                notification.SubscriberCount = _adder.Add(notification.SubscriberCount, C);
            }
        }

           public class NotificationAsyncHandlerA : INotificationAsyncHandler<Notification>
        {
            public Task HandleAsync(Notification notification, CancellationToken cancellationToken)
            {
                notification.SubscriberCount += A;
                return Task.CompletedTask;
            }
        }

        public class NotificationAsyncHandlerB : INotificationAsyncHandler<Notification>
        {
            public Task HandleAsync(Notification notification, CancellationToken cancellationToken)
            {
                notification.SubscriberCount += B;
                return Task.CompletedTask;
            }
        }

        public class NotificationAsyncHandlerC : INotificationAsyncHandler<Notification>
        {
            private readonly Adder _adder;

            public NotificationAsyncHandlerC(Adder adder) => _adder = adder;

            public Task HandleAsync(Notification notification, CancellationToken cancellationToken)
            {
                notification.SubscriberCount = _adder.Add(notification.SubscriberCount, C);
                return Task.CompletedTask;
            }
        }
        public class Adder
        {
            public int Add(int a, int b) => a + b;
        }
    }
}
