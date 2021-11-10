using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NPoco;
using NUnit.Framework;
using Sbn.Cms.Core;
using Sbn.Cms.Core.Configuration;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Infrastructure.Migrations.Install;
using Sbn.Cms.Infrastructure.Persistence;
using Sbn.Cms.Tests.Common.TestHelpers;
using Sbn.Cms.Tests.Common.Testing;
using Sbn.Cms.Tests.Integration.Testing;

namespace Sbn.Cms.Tests.Integration.Sbn.Infrastructure.Persistence
{
    [TestFixture]
    [SbnTest]
    [Platform("Win")]
    public class DatabaseBuilderTests : SbnIntegrationTest
    {
        private IDbProviderFactoryCreator DbProviderFactoryCreator => GetRequiredService<IDbProviderFactoryCreator>();
        private ISbnDatabaseFactory SbnDatabaseFactory => GetRequiredService<ISbnDatabaseFactory>();
        private IDatabaseCreator EmbeddedDatabaseCreator => GetRequiredService<IDatabaseCreator>();

        public DatabaseBuilderTests()
        {
            TestOptionAttributeBase.ScanAssemblies.Add(typeof(DatabaseBuilderTests).Assembly);
        }

        [Test]
        public void CreateDatabase()
        {
            var path = TestContext.CurrentContext.TestDirectory.Split("bin")[0];
            AppDomain.CurrentDomain.SetData("DataDirectory", path);
            const string dbFile = "DatabaseContextTests.sdf";
            // delete database file
            // NOTE: using a custom db file for this test since we're re-using the one created with BaseDatabaseFactoryTest
            var filePath = string.Concat(path, dbFile);
            if (File.Exists(filePath))
                File.Delete(filePath);

            var connectionString = $"Datasource=|DataDirectory|{dbFile};Flush Interval=1";

            SbnDatabaseFactory.Configure(connectionString, Constants.DbProviderNames.SqlCe);
            DbProviderFactoryCreator.CreateDatabase(Constants.DbProviderNames.SqlCe, connectionString);
            SbnDatabaseFactory.CreateDatabase();

            // test get database type (requires an actual database)
            using (var database = SbnDatabaseFactory.CreateDatabase())
            {
                var databaseType = database.DatabaseType;
                Assert.AreEqual(DatabaseType.SQLCe, databaseType);
            }

            // create application context
            //var appCtx = new ApplicationContext(
            //    _databaseFactory,
            //    new ServiceContext(migrationEntryService: Mock.Of<IMigrationEntryService>()),
            //    CacheHelper.CreateDisabledCacheHelper(),
            //    new ProfilingLogger(Mock.Of<ILogger>(), Mock.Of<IProfiler>()));

            // create the sbn database
            DatabaseSchemaCreator schemaHelper;
            using (var database = SbnDatabaseFactory.CreateDatabase())
            using (var transaction = database.GetTransaction())
            {
                schemaHelper = new DatabaseSchemaCreator(database, Mock.Of<ILogger<DatabaseSchemaCreator>>(), NullLoggerFactory.Instance, new SbnVersion(), Mock.Of<IEventAggregator>());
                schemaHelper.InitializeDatabaseSchema();
                transaction.Complete();
            }

            var sbnNodeTable = schemaHelper.TableExists("sbnNode");
            var sbnUserTable = schemaHelper.TableExists("sbnUser");
            var cmsTagsTable = schemaHelper.TableExists("cmsTags");

            Assert.That(sbnNodeTable, Is.True);
            Assert.That(sbnUserTable, Is.True);
            Assert.That(cmsTagsTable, Is.True);
        }

    }
}
