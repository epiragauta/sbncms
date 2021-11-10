using System.Threading.Tasks;
using Sbn.Cms.Core.Persistence.Repositories;

namespace Sbn.Cms.Core.Services
{
    public class InstallationService : IInstallationService
    {
        private readonly IInstallationRepository _installationRepository;

        public InstallationService(IInstallationRepository installationRepository)
        {
            _installationRepository = installationRepository;
        }

        public async Task LogInstall(InstallLog installLog)
        {
            await _installationRepository.SaveInstallLogAsync(installLog);
        }
    }
}
