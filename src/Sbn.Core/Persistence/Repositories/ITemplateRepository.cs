using System.Collections.Generic;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Persistence.Repositories
{
    public interface ITemplateRepository : IReadWriteQueryRepository<int, ITemplate>, IFileRepository
    {
        ITemplate Get(string alias);

        IEnumerable<ITemplate> GetAll(params string[] aliases);

        IEnumerable<ITemplate> GetChildren(int masterTemplateId);

        IEnumerable<ITemplate> GetDescendants(int masterTemplateId);
    }
}
