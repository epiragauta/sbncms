using System.Collections.Generic;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Services
{
    public interface IDomainService : IService
    {
        bool Exists(string domainName);
        Attempt<OperationResult> Delete(IDomain domain);
        IDomain GetByName(string name);
        IDomain GetById(int id);
        IEnumerable<IDomain> GetAll(bool includeWildcards);
        IEnumerable<IDomain> GetAssignedDomains(int contentId, bool includeWildcards);
        Attempt<OperationResult> Save(IDomain domainEntity);
    }
}
