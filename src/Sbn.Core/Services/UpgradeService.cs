using System.Threading.Tasks;
using Sbn.Cms.Core.Persistence.Repositories;
using Sbn.Cms.Core.Semver;

namespace Sbn.Cms.Core.Services
{
    public class UpgradeService : IUpgradeService
    {
        private readonly IUpgradeCheckRepository _upgradeCheckRepository;

        public UpgradeService(IUpgradeCheckRepository upgradeCheckRepository)
        {
            _upgradeCheckRepository = upgradeCheckRepository;
        }

        public async Task<UpgradeResult> CheckUpgrade(SemVersion version)
        {
            return await _upgradeCheckRepository.CheckUpgradeAsync(version);
        }
    }
}
