using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using Sbn.Cms.Core;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.DependencyInjection;
using Sbn.Cms.Core.IO;
using Sbn.Cms.Core.Packaging;
using Sbn.Cms.Core.PropertyEditors;
using Sbn.Cms.Core.Serialization;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Strings;
using Sbn.Cms.Infrastructure.Migrations;
using Sbn.Cms.Infrastructure.Packaging;
using Sbn.Cms.Tests.Common.Testing;
using Sbn.Cms.Tests.Integration.Testing;
using Sbn.Extensions;

namespace Sbn.Cms.Tests.Integration.Sbn.Core
{
    [TestFixture]
    [SbnTest(Database = SbnTestOptions.Database.NewSchemaPerTest)]
    public class RuntimeStateTests : SbnIntegrationTest
    {
        private protected IRuntimeState RuntimeState { get; private set; }

        public override void Configure(IApplicationBuilder app)
        {
            base.Configure(app);

            RuntimeState = Services.GetRequiredService<IRuntimeState>();
        }

        protected override void CustomTestSetup(ISbnBuilder builder)
        {
            PackageMigrationPlanCollectionBuilder migrations = builder.PackageMigrationPlans();
            migrations.Clear();
            migrations.Add<TestMigrationPlan>();
        }

        [Test]
        public void GivenPackageMigrationsExist_WhenLatestStateIsRegistered_ThenLevelIsRun()
        {
            // Add the final state to the keyvalue storage
            IKeyValueService keyValueService = Services.GetRequiredService<IKeyValueService>();
            keyValueService.SetValue(
                Constants.Conventions.Migrations.KeyValuePrefix + TestMigrationPlan.TestMigrationPlanName,
                TestMigrationPlan.TestMigrationFinalState.ToString());

            RuntimeState.DetermineRuntimeLevel();

            Assert.AreEqual(RuntimeLevel.Run, RuntimeState.Level);
            Assert.AreEqual(RuntimeLevelReason.Run, RuntimeState.Reason);
        }

        [Test]
        public void GivenPackageMigrationsExist_WhenUnattendedMigrations_ThenLevelIsRun()
        {
            RuntimeState.DetermineRuntimeLevel();

            Assert.AreEqual(RuntimeLevel.Run, RuntimeState.Level);
            Assert.AreEqual(RuntimeLevelReason.UpgradePackageMigrations, RuntimeState.Reason);
        }

        [Test]
        public void GivenPackageMigrationsExist_WhenNotUnattendedMigrations_ThenLevelIsRun()
        {
            var unattendedOptions = Services.GetRequiredService<IOptions<UnattendedSettings>>();
            unattendedOptions.Value.PackageMigrationsUnattended = false;

            RuntimeState.DetermineRuntimeLevel();

            Assert.AreEqual(RuntimeLevel.Run, RuntimeState.Level);
            Assert.AreEqual(RuntimeLevelReason.Run, RuntimeState.Reason);
        }

        private class TestMigrationPlan : PackageMigrationPlan
        {
            public const string TestMigrationPlanName = "Test";
            public static Guid TestMigrationFinalState => new Guid("BB02C392-4007-4A6C-A550-28BA2FF7E43D");

            public TestMigrationPlan() : base(TestMigrationPlanName)
            {
            }

            protected override void DefinePlan()
            {
                To<TestMigration>(TestMigrationFinalState);
            }
        }

        private class TestMigration : PackageMigrationBase
        {
            public TestMigration(IPackagingService packagingService, IMediaService mediaService, MediaFileManager mediaFileManager, MediaUrlGeneratorCollection mediaUrlGenerators, IShortStringHelper shortStringHelper, IContentTypeBaseServiceProvider contentTypeBaseServiceProvider, IMigrationContext context) : base(packagingService, mediaService, mediaFileManager, mediaUrlGenerators, shortStringHelper, contentTypeBaseServiceProvider, context)
            {
            }

            protected override void Migrate()
            {
                ImportPackage.FromEmbeddedResource<TestMigration>().Do();
            }
        }
    }
}
