using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Sbn.Cms.Core.Security
{
    /// <summary>
    /// Confirms whether a user is approved or not
    /// </summary>
    public class SbnUserConfirmation<TUser> : DefaultUserConfirmation<TUser>
        where TUser: SbnIdentityUser
    {
        public override Task<bool> IsConfirmedAsync(UserManager<TUser> manager, TUser user)
            => Task.FromResult(user.IsApproved);
    }
}
