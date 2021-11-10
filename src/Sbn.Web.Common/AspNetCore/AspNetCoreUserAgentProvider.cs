using Microsoft.AspNetCore.Http;
using Sbn.Cms.Core.Net;

namespace Sbn.Cms.Web.Common.AspNetCore
{
    public class AspNetCoreUserAgentProvider : IUserAgentProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AspNetCoreUserAgentProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserAgent()
        {
            return _httpContextAccessor.HttpContext.Request.Headers["User-Agent"].ToString();
        }
    }
}
