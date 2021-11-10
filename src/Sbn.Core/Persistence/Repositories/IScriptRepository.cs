using System.IO;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Persistence.Repositories
{
    public interface IScriptRepository : IReadRepository<string, IScript>, IWriteRepository<IScript>, IFileRepository, IFileWithFoldersRepository
    {
    }
}
