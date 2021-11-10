using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Sbn.Cms.Core.Configuration;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Infrastructure.Migrations.Install;
using Sbn.Cms.Tests.Common.Testing;
using Sbn.Cms.Tests.Integration.Testing;

namespace Sbn.Cms.Tests.Integration.Sbn.Infrastructure.Persistence
{
    [TestFixture]
    [SbnTest(Database = SbnTestOptions.Database.NewSchemaPerFixture)]
    public class SchemaValidationTest : SbnIntegrationTest
    {
        private ISbnVersion SbnVersion => GetRequiredService<ISbnVersion>();
        private IEventAggregator EventAggregator => GetRequiredService<IEventAggregator>();

        [Test]
        public void DatabaseSchemaCreation_Produces_DatabaseSchemaResult_With_Zero_Errors()
        {
            DatabaseSchemaResult result;

            using (var scope = ScopeProvider.CreateScope())
            {
                var schema = new DatabaseSchemaCreator(scope.Database, LoggerFactory.CreateLogger<DatabaseSchemaCreator>(), LoggerFactory, SbnVersion, EventAggregator);
                result = schema.ValidateSchema(DatabaseSchemaCreator.OrderedTables);
            }

            // Assert
            Assert.That(result.Errors.Count, Is.EqualTo(0));
        }
    }
}
