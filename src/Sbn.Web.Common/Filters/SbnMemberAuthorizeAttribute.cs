using Microsoft.AspNetCore.Mvc;

namespace Sbn.Cms.Web.Common.Filters
{
    /// <summary>
    /// Ensures authorization is successful for a website user (member).
    /// </summary>
    public class SbnMemberAuthorizeAttribute : TypeFilterAttribute
    {
        public SbnMemberAuthorizeAttribute() : this(string.Empty, string.Empty, string.Empty)
        {
        }

        public SbnMemberAuthorizeAttribute(string allowType, string allowGroup, string allowMembers) : base(typeof(SbnMemberAuthorizeFilter))
        {
            Arguments = new object[] { allowType, allowGroup, allowMembers};
        }

    }
}
