using System.Security.Principal;
using Sbn.Cms.Infrastructure.Security;

namespace Sbn.Cms.Core.Security
{
    /// <summary>
    /// The user manager for the back office
    /// </summary>
    public interface IBackOfficeUserManager : ISbnUserManager<BackOfficeIdentityUser>
    {
        void NotifyForgotPasswordRequested(IPrincipal currentUser, string userId);
        void NotifyForgotPasswordChanged(IPrincipal currentUser, string userId);
        SignOutSuccessResult NotifyLogoutSuccess(IPrincipal currentUser, string userId);
    }
}
