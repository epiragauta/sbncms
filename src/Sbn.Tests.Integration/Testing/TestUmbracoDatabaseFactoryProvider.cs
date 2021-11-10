// Copyright (c) Sbn.
// See LICENSE for more details.

using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Infrastructure.Migrations.Install;
using Sbn.Cms.Infrastructure.Persistence;
using Sbn.Cms.Infrastructure.Persistence.Mappers;

namespace Sbn.Cms.Tests.Integration.Testing
{
    /// <summary>
    /// I want to be able to create a database for integration testsing without setting the connection string on the
    /// singleton database factory forever.
    /// </summary>
    public class TestSbnDatabaseFactoryProvider
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IOptions<GlobalSettings> _globalSettings;
        private readonly IOptionsMonitor<ConnectionStrings> _connectionStrings;
        private readonly IMapperCollection _mappers;
        private readonly IDbProviderFactoryCreator _dbProviderFactoryCreator;
        private readonly DatabaseSchemaCreatorFactory _databaseSchemaCreatorFactory;
        private readonly NPocoMapperCollection _npocoMappers;

        public TestSbnDatabaseFactoryProvider(
            ILoggerFactory loggerFactory,
            IOptions<GlobalSettings> globalSettings,
            IOptionsMonitor<ConnectionStrings> connectionStrings,
            IMapperCollection mappers,
            IDbProviderFactoryCreator dbProviderFactoryCreator,
            DatabaseSchemaCreatorFactory databaseSchemaCreatorFactory,
            NPocoMapperCollection npocoMappers)
        {
            _loggerFactory = loggerFactory;
            _globalSettings = globalSettings;
            _connectionStrings = connectionStrings;
            _mappers = mappers;
            _dbProviderFactoryCreator = dbProviderFactoryCreator;
            _databaseSchemaCreatorFactory = databaseSchemaCreatorFactory;
            _npocoMappers = npocoMappers;
        }

        public ISbnDatabaseFactory Create()
            => new SbnDatabaseFactory(
                _loggerFactory.CreateLogger<SbnDatabaseFactory>(),
                _loggerFactory,
                _globalSettings,
                _connectionStrings,
                _mappers,
                _dbProviderFactoryCreator,
                _databaseSchemaCreatorFactory,
                _npocoMappers);
    }
}
