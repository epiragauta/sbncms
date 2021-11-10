using System;
using System.Collections.Generic;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Persistence.Repositories
{
    public interface IDictionaryRepository : IReadWriteQueryRepository<int, IDictionaryItem>
    {
        IDictionaryItem Get(Guid uniqueId);
        IDictionaryItem Get(string key);
        IEnumerable<IDictionaryItem> GetDictionaryItemDescendants(Guid? parentId);
        Dictionary<string, Guid> GetDictionaryItemKeyMap();
    }
}
