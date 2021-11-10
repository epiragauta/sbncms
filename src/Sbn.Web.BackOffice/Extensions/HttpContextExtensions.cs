using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Sbn.Cms.Core.Security;

namespace Sbn.Extensions
{
    public static class HttpContextExtensions
    {
        public static void SetExternalLoginProviderErrors(this HttpContext httpContext, BackOfficeExternalLoginProviderErrors errors)
            => httpContext.Items[nameof(BackOfficeExternalLoginProviderErrors)] = errors;

        public static BackOfficeExternalLoginProviderErrors GetExternalLoginProviderErrors(this HttpContext httpContext)
            => httpContext.Items[nameof(BackOfficeExternalLoginProviderErrors)] as BackOfficeExternalLoginProviderErrors;

    }
}
