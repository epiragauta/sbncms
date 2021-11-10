// Copyright (c) Sbn.
// See LICENSE for more details.

using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Sbn.Cms.Infrastructure.Persistence;

namespace Sbn.Cms.Tests.Integration.Testing
{
    public class TestDatabaseFactory
    {
        public static ITestDatabase Create(TestDatabaseSettings settings, string filesPath, ILoggerFactory loggerFactory, TestSbnDatabaseFactoryProvider dbFactory)
        {
            string connectionString = Environment.GetEnvironmentVariable("SbnIntegrationTestConnectionString");

            return string.IsNullOrEmpty(connectionString)
                ? CreateLocalDb(settings, filesPath, loggerFactory, dbFactory)
                : CreateSqlDeveloper(settings, loggerFactory, dbFactory, connectionString);
        }

        private static ITestDatabase CreateLocalDb(TestDatabaseSettings settings, string filesPath, ILoggerFactory loggerFactory, TestSbnDatabaseFactoryProvider dbFactory)
        {
            if (!Directory.Exists(filesPath))
            {
                Directory.CreateDirectory(filesPath);
            }

            var localDb = new LocalDb();

            if (!localDb.IsAvailable)
            {
                throw new InvalidOperationException("LocalDB is not available.");
            }

            return new LocalDbTestDatabase(settings, loggerFactory, localDb, filesPath, dbFactory.Create());
        }

        private static ITestDatabase CreateSqlDeveloper(TestDatabaseSettings settings, ILoggerFactory loggerFactory, TestSbnDatabaseFactoryProvider dbFactory, string connectionString)
        {
            // NOTE: Example setup for Linux box.
            // $ export SA_PASSWORD=Foobar123!
            // $ export SbnIntegrationTestConnectionString="Server=localhost,1433;User Id=sa;Password=$SA_PASSWORD;"
            // $ docker run -e 'ACCEPT_EULA=Y' -e "SA_PASSWORD=$SA_PASSWORD" -e 'MSSQL_PID=Developer' -p 1433:1433 -d mcr.microsoft.com/mssql/server:2017-latest-ubuntu
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("ENV: SbnIntegrationTestConnectionString is not set");
            }

            return new SqlDeveloperTestDatabase(settings, loggerFactory, dbFactory.Create(), connectionString);
        }
    }
}
