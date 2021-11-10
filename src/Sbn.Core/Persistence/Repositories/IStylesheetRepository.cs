using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Persistence.Repositories
{
    public interface IStylesheetRepository : IReadRepository<string, IStylesheet>, IWriteRepository<IStylesheet>, IFileRepository, IFileWithFoldersRepository
    {
    }
}
