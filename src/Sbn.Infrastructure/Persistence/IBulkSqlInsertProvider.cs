using System.Collections.Generic;

namespace Sbn.Cms.Infrastructure.Persistence
{
    public interface IBulkSqlInsertProvider
    {
        string ProviderName { get; }
        int BulkInsertRecords<T>(ISbnDatabase database, IEnumerable<T> records);
    }
}
