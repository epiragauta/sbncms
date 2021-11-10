using System.Threading.Tasks;

namespace Sbn.Cms.Core.Services
{
    public interface IInstallationService
    {
        Task LogInstall(InstallLog installLog);
    }
}
