using System.Threading.Tasks;
using Sbn.Cms.Core.Semver;

namespace Sbn.Cms.Core.Services
{
    public interface IUpgradeService
    {
        Task<UpgradeResult> CheckUpgrade(SemVersion version);
    }
}
