using System.Data.Common;
using Sbn.Cms.Infrastructure.Persistence.SqlSyntax;

namespace Sbn.Cms.Infrastructure.Persistence
{

    public interface IDbProviderFactoryCreator
    {
        DbProviderFactory CreateFactory(string providerName);
        ISqlSyntaxProvider GetSqlSyntaxProvider(string providerName);
        IBulkSqlInsertProvider CreateBulkSqlInsertProvider(string providerName);
        void CreateDatabase(string providerName, string connectionString);
        NPocoMapperCollection ProviderSpecificMappers(string providerName);
    }
}
