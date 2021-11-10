using System.Threading.Tasks;
using Sbn.Cms.Core;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Security;

namespace Sbn.Cms.Web.Common.Security
{
    public interface IPasswordChanger<TUser> where TUser : SbnIdentityUser
    {
        public Task<Attempt<PasswordChangedModel>> ChangePasswordWithIdentityAsync(
            ChangingPasswordModel passwordModel,
            ISbnUserManager<TUser> userMgr);
    }
}
