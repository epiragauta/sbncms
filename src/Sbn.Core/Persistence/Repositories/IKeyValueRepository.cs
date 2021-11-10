using System.Collections.Generic;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Persistence.Repositories
{
    public interface IKeyValueRepository : IReadRepository<string, IKeyValue>, IWriteRepository<IKeyValue>
    {
        /// <summary>
        /// Returns key/value pairs for all keys with the specified prefix.
        /// </summary>
        /// <param name="keyPrefix"></param>
        /// <returns></returns>
        IReadOnlyDictionary<string, string> FindByKeyPrefix(string keyPrefix);
    }
}
