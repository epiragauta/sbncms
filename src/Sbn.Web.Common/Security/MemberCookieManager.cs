using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Sbn.Cms.Core;
using Sbn.Cms.Core.Routing;
using Sbn.Cms.Core.Services;

namespace Sbn.Cms.Web.Common.Security
{
    /// <summary>
    /// A custom cookie manager for members to ensure that cookie auth does not occur for any back office requests
    /// </summary>
    public class MemberCookieManager : ChunkingCookieManager, ICookieManager
    {
        private readonly IRuntimeState _runtime;
        private readonly SbnRequestPaths _sbnRequestPaths;

        public MemberCookieManager(IRuntimeState runtime, SbnRequestPaths sbnRequestPaths)
        {
            _runtime = runtime ?? throw new ArgumentNullException(nameof(runtime));
            _sbnRequestPaths = sbnRequestPaths ?? throw new ArgumentNullException(nameof(sbnRequestPaths));
        }

        /// <summary>
        /// Determines if we should authenticate the request
        /// </summary>
        /// <returns>true if the request should be authenticated</returns>
        /// <remarks>
        /// We auth the request when it is not a back office request and when the runtime level is Run
        /// </remarks>
        public bool ShouldAuthenticateRequest(string absPath)
        {
            // Do not authenticate the request if we are not running.
            // Else this can cause problems especially if the members DB table needs upgrades
            // because when authing, the member db table will be read and we'll get exceptions.
            if (_runtime.Level != RuntimeLevel.Run)
            {
                return false;
            }

            if (// check back office
                _sbnRequestPaths.IsBackOfficeRequest(absPath)

                // check installer
                || _sbnRequestPaths.IsInstallerRequest(absPath))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Explicitly implement this so that we filter the request
        /// </summary>
        /// <inheritdoc/>
        string ICookieManager.GetRequestCookie(HttpContext context, string key)
        {
            var absPath = context.Request.Path;

            return ShouldAuthenticateRequest(absPath) == false

                // Don't auth request, don't return a cookie
                ? null

                // Return the default implementation
                : GetRequestCookie(context, key);
        }
    }
}
