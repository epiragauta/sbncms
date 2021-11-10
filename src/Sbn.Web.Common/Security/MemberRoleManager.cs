using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Sbn.Cms.Core.Security;

namespace Sbn.Cms.Web.Common.Security
{
    public class MemberRoleManager : RoleManager<SbnIdentityRole>, IMemberRoleManager
    {
        public MemberRoleManager(
            IRoleStore<SbnIdentityRole> store,
            IEnumerable<IRoleValidator<SbnIdentityRole>> roleValidators,
            IdentityErrorDescriber errors,
            ILogger<MemberRoleManager> logger)
            : base(store, roleValidators, new NoopLookupNormalizer(), errors, logger) { }

        IEnumerable<SbnIdentityRole> IMemberRoleManager.Roles => base.Roles.ToList();
    }
}
