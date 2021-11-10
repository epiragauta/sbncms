using Microsoft.AspNetCore.Http;
using Sbn.Cms.Core.Net;

namespace Sbn.Cms.Web.Common.AspNetCore
{
    public class AspNetCoreIpResolver : IIpResolver
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AspNetCoreIpResolver(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentRequestIpAddress() => _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? string.Empty;
    }
}
