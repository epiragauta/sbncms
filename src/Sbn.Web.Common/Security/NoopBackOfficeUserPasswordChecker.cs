using System.Threading.Tasks;
using Sbn.Cms.Core.Security;

namespace Sbn.Cms.Web.Common.Security
{
    public class NoopBackOfficeUserPasswordChecker : IBackOfficeUserPasswordChecker
    {
        public Task<BackOfficeUserPasswordCheckerResult> CheckPasswordAsync(BackOfficeIdentityUser user, string password)
            => Task.FromResult(BackOfficeUserPasswordCheckerResult.FallbackToDefaultChecker);
    }
}
