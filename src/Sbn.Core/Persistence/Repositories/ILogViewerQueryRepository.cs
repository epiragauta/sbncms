using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Persistence.Repositories
{
    public interface ILogViewerQueryRepository : IReadWriteQueryRepository<int, ILogViewerQuery>
    {
        ILogViewerQuery GetByName(string name);
    }
}
