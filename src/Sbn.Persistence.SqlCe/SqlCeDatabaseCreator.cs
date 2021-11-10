using Sbn.Cms.Infrastructure.Persistence;
using Constants = Sbn.Cms.Core.Constants;

namespace Sbn.Cms.Persistence.SqlCe
{
    public class SqlCeDatabaseCreator : IDatabaseCreator
    {
        public string ProviderName => Constants.DatabaseProviders.SqlCe;

        public void Create(string connectionString)
        {
            using var engine = new System.Data.SqlServerCe.SqlCeEngine(connectionString);
            engine.CreateDatabase();
        }
    }
}
