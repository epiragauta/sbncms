// Copyright (c) Sbn.
// See LICENSE for more details.

using System;
using NUnit.Framework;
using Sbn.Cms.Core.DependencyInjection;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Notifications;
using Sbn.Cms.Core.Scoping;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Tests.Common.Builders;
using Sbn.Cms.Tests.Common.Testing;
using Sbn.Cms.Tests.Integration.Testing;

namespace Sbn.Cms.Tests.Integration.Sbn.Infrastructure.Scoping
{
    [TestFixture]
    [SbnTest(Database = SbnTestOptions.Database.NewSchemaPerTest)]
    public class SuppressNotificationsTests : SbnIntegrationTest
    {
        private IContentService ContentService => GetRequiredService<IContentService>();
        private IContentTypeService ContentTypeService => GetRequiredService<IContentTypeService>();
        private IMediaTypeService MediaTypeService => GetRequiredService<IMediaTypeService>();
        private IMediaService MediaService => GetRequiredService<IMediaService>();

        protected override void CustomTestSetup(ISbnBuilder builder)
        {
            base.CustomTestSetup(builder);

            builder.AddNotificationHandler<ContentSavingNotification, TestContentNotificationHandler>();
            builder.AddNotificationHandler<ContentTypeSavingNotification, TestContentTypeNotificationHandler>();
            builder.AddNotificationHandler<MediaSavedNotification, TestMediaNotificationHandler>();
        }

        [Test]
        public void GivenScope_WhenNotificationsSuppressed_ThenNotificationsDoNotExecute()
        {
            using IScope scope = ScopeProvider.CreateScope(autoComplete: true);
            using IDisposable _ = scope.Notifications.Suppress();

            ContentType contentType = ContentTypeBuilder.CreateBasicContentType();
            ContentTypeService.Save(contentType);
            Content content = ContentBuilder.CreateBasicContent(contentType);
            ContentService.Save(content);
        }

        [Test]
        public void GivenNestedScope_WhenOuterNotificationsSuppressed_ThenNotificationsDoNotExecute()
        {
            using (IScope parentScope = ScopeProvider.CreateScope(autoComplete: true))
            {
                using IDisposable _ = parentScope.Notifications.Suppress();

                using (IScope childScope = ScopeProvider.CreateScope(autoComplete: true))
                {
                    ContentType contentType = ContentTypeBuilder.CreateBasicContentType();
                    ContentTypeService.Save(contentType);
                    Content content = ContentBuilder.CreateBasicContent(contentType);
                    ContentService.Save(content);
                }
            }
        }

        [Test]
        public void GivenSuppressedNotifications_WhenDisposed_ThenNotificationsExecute()
        {
            int asserted = 0;
            using (IScope scope = ScopeProvider.CreateScope(autoComplete: true))
            {
                using IDisposable suppressed = scope.Notifications.Suppress();

                MediaType mediaType = MediaTypeBuilder.CreateImageMediaType("test");
                MediaTypeService.Save(mediaType);

                suppressed.Dispose();

                asserted = TestContext.CurrentContext.AssertCount;
                Media media = MediaBuilder.CreateMediaImage(mediaType, -1);
                MediaService.Save(media);
            }   

            Assert.AreEqual(asserted + 1, TestContext.CurrentContext.AssertCount);
        }

        [Test]
        public void GivenSuppressedNotificationsOnParent_WhenChildSuppresses_ThenExceptionIsThrown()
        {
            using (IScope parentScope = ScopeProvider.CreateScope(autoComplete: true))
            using (IDisposable parentSuppressed = parentScope.Notifications.Suppress())
            {
                using (IScope childScope = ScopeProvider.CreateScope(autoComplete: true))                
                {
                    Assert.Throws<InvalidOperationException>(() => childScope.Notifications.Suppress());
                }
            }
        }

        private class TestContentNotificationHandler : INotificationHandler<ContentSavingNotification>
        {
            public void Handle(ContentSavingNotification notification)
                => Assert.Fail("Notification was sent");
        }

        private class TestMediaNotificationHandler : INotificationHandler<MediaSavedNotification>
        {
            public void Handle(MediaSavedNotification notification)
                => Assert.IsNotNull(notification);
        }

        private class TestContentTypeNotificationHandler : INotificationHandler<ContentTypeSavingNotification>
        {
            public void Handle(ContentTypeSavingNotification notification)
                => Assert.Fail("Notification was sent");
        }
    }
}
