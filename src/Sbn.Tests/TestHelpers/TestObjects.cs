using System;
using System.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NPoco;
using Sbn.Cms.Core.Cache;
using Sbn.Cms.Core.Configuration;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.IO;
using Sbn.Cms.Core.Scoping;
using Sbn.Cms.Infrastructure.Migrations.Install;
using Sbn.Cms.Infrastructure.Persistence;
using Sbn.Cms.Infrastructure.Persistence.Mappers;
using Sbn.Cms.Infrastructure.Persistence.SqlSyntax;
using Sbn.Cms.Persistence.SqlCe;
using Sbn.Web.Composing;
using Constants = Sbn.Cms.Core.Constants;

namespace Sbn.Tests.TestHelpers
{
    /// <summary>
    /// Provides objects for tests.
    /// </summary>
    internal partial class TestObjects
    {

        public TestObjects()
        {
        }

        /// <summary>
        /// Gets an SbnDatabase.
        /// </summary>
        /// <param name="logger">A logger.</param>
        /// <returns>An SbnDatabase.</returns>
        /// <remarks>This is just a void database that has no actual database but pretends to have an open connection
        /// that can begin a transaction.</remarks>
        public SbnDatabase GetSbnSqlCeDatabase(ILogger<SbnDatabase> logger)
        {
            var syntax = new SqlCeSyntaxProvider(Options.Create(new GlobalSettings()));
            var connection = GetDbConnection();
            var sqlContext = new SqlContext(syntax, DatabaseType.SQLCe, Mock.Of<IPocoDataFactory>());
            return new SbnDatabase(connection, sqlContext, logger, TestHelper.BulkSqlInsertProvider);
        }

        /// <summary>
        /// Gets an SbnDatabase.
        /// </summary>
        /// <param name="logger">A logger.</param>
        /// <returns>An SbnDatabase.</returns>
        /// <remarks>This is just a void database that has no actual database but pretends to have an open connection
        /// that can begin a transaction.</remarks>
        public SbnDatabase GetSbnSqlServerDatabase(ILogger<SbnDatabase> logger)
        {
            var syntax = new SqlServerSyntaxProvider(Options.Create(new GlobalSettings())); // do NOT try to get the server's version!
            var connection = GetDbConnection();
            var sqlContext = new SqlContext(syntax, DatabaseType.SqlServer2008, Mock.Of<IPocoDataFactory>());
            return new SbnDatabase(connection, sqlContext, logger, TestHelper.BulkSqlInsertProvider);
        }

        public IScopeProvider GetScopeProvider(ILoggerFactory loggerFactory, FileSystems fileSystems = null, ISbnDatabaseFactory databaseFactory = null)
        {
            var globalSettings = Options.Create(new GlobalSettings());
            var connectionString = ConfigurationManager.ConnectionStrings[Constants.System.SbnConnectionName].ConnectionString;
            var connectionStrings = Options.Create(new ConnectionStrings { SbnConnectionString = new ConfigConnectionString(Constants.System.SbnConnectionName, connectionString) });
            var coreDebugSettings = new CoreDebugSettings();

            if (databaseFactory == null)
            {
                // var mappersBuilder = new MapperCollectionBuilder(Current.Container); // FIXME:
                // mappersBuilder.AddCore();
                // var mappers = mappersBuilder.CreateCollection();
                var mappers = Current.Factory.GetRequiredService<IMapperCollection>();
                databaseFactory = new SbnDatabaseFactory(
                    loggerFactory.CreateLogger<SbnDatabaseFactory>(),
                    loggerFactory,
                    globalSettings,
                    connectionStrings,
                    new Lazy<IMapperCollection>(() => mappers),
                    TestHelper.DbProviderFactoryCreator,
                    new DatabaseSchemaCreatorFactory(Mock.Of<ILogger<DatabaseSchemaCreator>>(),loggerFactory, new SbnVersion(), Mock.Of<IEventAggregator>()));
            }

            fileSystems ??= new FileSystems(loggerFactory, TestHelper.IOHelper, globalSettings, TestHelper.GetHostingEnvironment());
            var coreDebug = TestHelper.CoreDebugSettings;
            var mediaFileManager = Mock.Of<MediaFileManager>();
            var eventAggregator = Mock.Of<IEventAggregator>();
            return new ScopeProvider(databaseFactory, fileSystems, Options.Create(coreDebugSettings), mediaFileManager, loggerFactory.CreateLogger<ScopeProvider>(), loggerFactory, NoAppCache.Instance, eventAggregator);
        }

    }
}
