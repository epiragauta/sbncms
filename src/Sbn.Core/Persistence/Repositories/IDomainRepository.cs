using System.Collections.Generic;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Persistence.Repositories
{
    public interface IDomainRepository : IReadWriteQueryRepository<int, IDomain>
    {
        IDomain GetByName(string domainName);
        bool Exists(string domainName);
        IEnumerable<IDomain> GetAll(bool includeWildcards);
        IEnumerable<IDomain> GetAssignedDomains(int contentId, bool includeWildcards);
    }
}
