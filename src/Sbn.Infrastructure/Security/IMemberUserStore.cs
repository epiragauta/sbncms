using Microsoft.AspNetCore.Identity;
using Sbn.Cms.Core.Models.PublishedContent;

namespace Sbn.Cms.Core.Security
{
    /// <summary>
    /// A custom user store that uses Sbn member data
    /// </summary>
    public interface IMemberUserStore : IUserStore<MemberIdentityUser>
    {
        IPublishedContent GetPublishedMember(MemberIdentityUser user);
    }
}
