using System.Collections.Generic;
using Sbn.Cms.Core.Security;

namespace Sbn.Cms.Web.Common.Security
{
    public interface IMemberRoleManager
    {
        IEnumerable<SbnIdentityRole> Roles { get; }
    }
}
