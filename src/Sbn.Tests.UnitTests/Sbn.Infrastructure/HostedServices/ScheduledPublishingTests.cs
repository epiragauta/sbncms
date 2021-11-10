// Copyright (c) Sbn.
// See LICENSE for more details.

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Sbn.Cms.Core;
using Sbn.Cms.Core.Runtime;
using Sbn.Cms.Core.Security;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Sync;
using Sbn.Cms.Core.Web;
using Sbn.Cms.Infrastructure;
using Sbn.Cms.Infrastructure.HostedServices;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Infrastructure.HostedServices
{
    [TestFixture]
    public class ScheduledPublishingTests
    {
        private Mock<IContentService> _mockContentService;
        private Mock<ILogger<ScheduledPublishing>> _mockLogger;

        [Test]
        public async Task Does_Not_Execute_When_Not_Enabled()
        {
            ScheduledPublishing sut = CreateScheduledPublishing(enabled: false);
            await sut.PerformExecuteAsync(null);
            VerifyScheduledPublishingNotPerformed();
        }

        [TestCase(RuntimeLevel.Boot)]
        [TestCase(RuntimeLevel.Install)]
        [TestCase(RuntimeLevel.Unknown)]
        [TestCase(RuntimeLevel.Upgrade)]
        [TestCase(RuntimeLevel.BootFailed)]
        public async Task Does_Not_Execute_When_Runtime_State_Is_Not_Run(RuntimeLevel runtimeLevel)
        {
            ScheduledPublishing sut = CreateScheduledPublishing(runtimeLevel: runtimeLevel);
            await sut.PerformExecuteAsync(null);
            VerifyScheduledPublishingNotPerformed();
        }

        [Test]
        public async Task Does_Not_Execute_When_Server_Role_Is_Subscriber()
        {
            ScheduledPublishing sut = CreateScheduledPublishing(serverRole: ServerRole.Subscriber);
            await sut.PerformExecuteAsync(null);
            VerifyScheduledPublishingNotPerformed();
        }

        [Test]
        public async Task Does_Not_Execute_When_Server_Role_Is_Unknown()
        {
            ScheduledPublishing sut = CreateScheduledPublishing(serverRole: ServerRole.Unknown);
            await sut.PerformExecuteAsync(null);
            VerifyScheduledPublishingNotPerformed();
        }

        [Test]
        public async Task Does_Not_Execute_When_Not_Main_Dom()
        {
            ScheduledPublishing sut = CreateScheduledPublishing(isMainDom: false);
            await sut.PerformExecuteAsync(null);
            VerifyScheduledPublishingNotPerformed();
        }

        [Test]
        public async Task Executes_And_Performs_Scheduled_Publishing()
        {
            ScheduledPublishing sut = CreateScheduledPublishing();
            await sut.PerformExecuteAsync(null);
            VerifyScheduledPublishingPerformed();
        }

        private ScheduledPublishing CreateScheduledPublishing(
            bool enabled = true,
            RuntimeLevel runtimeLevel = RuntimeLevel.Run,
            ServerRole serverRole = ServerRole.Single,
            bool isMainDom = true)
        {
            if (enabled)
            {
                Suspendable.ScheduledPublishing.Resume();
            }
            else
            {
                Suspendable.ScheduledPublishing.Suspend();
            }

            var mockRunTimeState = new Mock<IRuntimeState>();
            mockRunTimeState.SetupGet(x => x.Level).Returns(runtimeLevel);

            var mockServerRegistrar = new Mock<IServerRoleAccessor>();
            mockServerRegistrar.Setup(x => x.CurrentServerRole).Returns(serverRole);

            var mockMainDom = new Mock<IMainDom>();
            mockMainDom.SetupGet(x => x.IsMainDom).Returns(isMainDom);

            _mockContentService = new Mock<IContentService>();

            var mockSbnContextFactory = new Mock<ISbnContextFactory>();
            mockSbnContextFactory.Setup(x => x.EnsureSbnContext()).Returns(new SbnContextReference(null, false, null));

            _mockLogger = new Mock<ILogger<ScheduledPublishing>>();

            var mockServerMessenger = new Mock<IServerMessenger>();

            return new ScheduledPublishing(
                mockRunTimeState.Object,
                mockMainDom.Object,
                mockServerRegistrar.Object,
                _mockContentService.Object,
                mockSbnContextFactory.Object,
                _mockLogger.Object,
                mockServerMessenger.Object);
        }

        private void VerifyScheduledPublishingNotPerformed() => VerifyScheduledPublishingPerformed(Times.Never());

        private void VerifyScheduledPublishingPerformed() => VerifyScheduledPublishingPerformed(Times.Once());

        private void VerifyScheduledPublishingPerformed(Times times) => _mockContentService.Verify(x => x.PerformScheduledPublish(It.IsAny<DateTime>()), times);
    }
}
