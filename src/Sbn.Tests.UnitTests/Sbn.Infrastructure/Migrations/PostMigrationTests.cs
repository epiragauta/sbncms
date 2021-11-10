// Copyright (c) Sbn.
// See LICENSE for more details.

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using NPoco;
using NUnit.Framework;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Migrations;
using Sbn.Cms.Core.Scoping;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Infrastructure.Migrations;
using Sbn.Cms.Infrastructure.Migrations.Upgrade;
using Sbn.Cms.Infrastructure.Persistence;
using Sbn.Cms.Infrastructure.Persistence.SqlSyntax;
using Sbn.Cms.Tests.Common.TestHelpers;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Infrastructure.Migrations
{
    [TestFixture]
    public class PostMigrationTests
    {
        private static readonly ILoggerFactory s_loggerFactory = NullLoggerFactory.Instance;
        private IMigrationPlanExecutor GetMigrationPlanExecutor(IScopeProvider scopeProvider, IMigrationBuilder builder)
            => new MigrationPlanExecutor(scopeProvider, s_loggerFactory, builder);

        [Test]
        public void ExecutesPlanPostMigration()
        {
            IMigrationBuilder builder = Mock.Of<IMigrationBuilder>();
            Mock.Get(builder)
                .Setup(x => x.Build(It.IsAny<Type>(), It.IsAny<IMigrationContext>()))
                .Returns<Type, IMigrationContext>((t, c) =>
                {
                    switch (t.Name)
                    {
                        case nameof(NoopMigration):
                            return new NoopMigration(c);
                        case nameof(TestPostMigration):
                            return new TestPostMigration(c);
                        default:
                            throw new NotSupportedException();
                    }
                });

            var database = new TestDatabase();
            IScope scope = Mock.Of<IScope>(x => x.Notifications == Mock.Of<IScopedNotificationPublisher>());
            Mock.Get(scope)
                .Setup(x => x.Database)
                .Returns(database);

            var sqlContext = new SqlContext(
                new SqlServerSyntaxProvider(Options.Create(new GlobalSettings())),
                DatabaseType.SQLCe,
                Mock.Of<IPocoDataFactory>());
            var scopeProvider = new MigrationTests.TestScopeProvider(scope) { SqlContext = sqlContext };

            MigrationPlan plan = new MigrationPlan("Test")
                .From(string.Empty).To("done");

            plan.AddPostMigration<TestPostMigration>();
            TestPostMigration.MigrateCount = 0;

            var upgrader = new Upgrader(plan);
            IMigrationPlanExecutor executor = GetMigrationPlanExecutor(scopeProvider, builder);
            upgrader.Execute(
                executor,
                scopeProvider,
                Mock.Of<IKeyValueService>());

            Assert.AreEqual(1, TestPostMigration.MigrateCount);
        }

        [Test]
        public void MigrationCanAddPostMigration()
        {
            IMigrationBuilder builder = Mock.Of<IMigrationBuilder>();
            Mock.Get(builder)
                .Setup(x => x.Build(It.IsAny<Type>(), It.IsAny<IMigrationContext>()))
                .Returns<Type, IMigrationContext>((t, c) =>
                {
                    switch (t.Name)
                    {
                        case nameof(NoopMigration):
                            return new NoopMigration(c);
                        case nameof(TestMigration):
                            return new TestMigration(c);
                        case nameof(TestPostMigration):
                            return new TestPostMigration(c);
                        default:
                            throw new NotSupportedException();
                    }
                });

            var database = new TestDatabase();
            IScope scope = Mock.Of<IScope>(x => x.Notifications == Mock.Of<IScopedNotificationPublisher>());
            Mock.Get(scope)
                .Setup(x => x.Database)
                .Returns(database);

            var sqlContext = new SqlContext(
                new SqlServerSyntaxProvider(Options.Create(new GlobalSettings())),
                DatabaseType.SQLCe,
                Mock.Of<IPocoDataFactory>());
            var scopeProvider = new MigrationTests.TestScopeProvider(scope) { SqlContext = sqlContext };

            MigrationPlan plan = new MigrationPlan("Test")
                .From(string.Empty).To<TestMigration>("done");

            TestMigration.MigrateCount = 0;
            TestPostMigration.MigrateCount = 0;

            new MigrationContext(plan, database, s_loggerFactory.CreateLogger<MigrationContext>());

            var upgrader = new Upgrader(plan);
            IMigrationPlanExecutor executor = GetMigrationPlanExecutor(scopeProvider, builder);
            upgrader.Execute(
                executor,
                scopeProvider,
                Mock.Of<IKeyValueService>());

            Assert.AreEqual(1, TestMigration.MigrateCount);
            Assert.AreEqual(1, TestPostMigration.MigrateCount);
        }

        public class TestMigration : MigrationBase
        {
            public TestMigration(IMigrationContext context)
                : base(context)
            {
            }

            public static int MigrateCount { get; set; }

            protected override void Migrate()
            {
                MigrateCount++;

                Context.AddPostMigration<TestPostMigration>();
            }
        }

        public class TestPostMigration : MigrationBase
        {
            public TestPostMigration(IMigrationContext context) : base(context)
            {
            }

            public static int MigrateCount { get; set; }

            protected override void Migrate() => MigrateCount++;
        }
    }
}
