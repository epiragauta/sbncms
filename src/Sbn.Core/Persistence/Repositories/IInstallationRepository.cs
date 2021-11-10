using System.Threading.Tasks;

namespace Sbn.Cms.Core.Persistence.Repositories
{
    public interface IInstallationRepository
    {
        Task SaveInstallLogAsync(InstallLog installLog);
    }
}
