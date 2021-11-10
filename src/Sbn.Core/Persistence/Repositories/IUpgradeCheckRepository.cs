using System.Threading.Tasks;
using Sbn.Cms.Core.Semver;

namespace Sbn.Cms.Core.Persistence.Repositories
{
    public interface IUpgradeCheckRepository
    {
        Task<UpgradeResult> CheckUpgradeAsync(SemVersion version);
    }
}
