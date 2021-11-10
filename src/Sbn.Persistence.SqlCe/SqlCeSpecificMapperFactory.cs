using Sbn.Cms.Core;
using Sbn.Cms.Infrastructure.Persistence;

namespace Sbn.Cms.Persistence.SqlCe
{
    public class SqlCeSpecificMapperFactory : IProviderSpecificMapperFactory
    {
        public string ProviderName => Constants.DatabaseProviders.SqlCe;
        public NPocoMapperCollection Mappers => new NPocoMapperCollection(() => new[] {new SqlCeImageMapper()});
    }
}
