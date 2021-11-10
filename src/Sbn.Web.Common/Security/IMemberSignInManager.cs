using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Sbn.Cms.Core.Security;

namespace Sbn.Cms.Web.Common.Security
{
    public interface IMemberSignInManager
    {
        // TODO: We could have a base interface for these to share with IBackOfficeSignInManager
        Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure);
        Task SignInAsync(MemberIdentityUser user, bool isPersistent, string authenticationMethod = null);
        Task SignOutAsync();
    }
}
