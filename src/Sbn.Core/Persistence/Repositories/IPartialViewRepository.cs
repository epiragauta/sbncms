using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Persistence.Repositories
{
    public interface IPartialViewRepository : IReadRepository<string, IPartialView>, IWriteRepository<IPartialView>, IFileRepository, IFileWithFoldersRepository
    {
    }
}
