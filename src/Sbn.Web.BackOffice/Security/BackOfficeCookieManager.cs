using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Sbn.Cms.Core;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.Routing;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Web;
using Sbn.Extensions;

namespace Sbn.Cms.Web.BackOffice.Security
{
    /// <summary>
    /// A custom cookie manager that is used to read the cookie from the request.
    /// </summary>
    /// <remarks>
    /// Sbn's back office cookie needs to be read on two paths: /sbn and /install, therefore we cannot just set the cookie path to be /sbn,
    /// instead we'll specify our own cookie manager and return null if the request isn't for an acceptable path.
    /// </remarks>
    public class BackOfficeCookieManager : ChunkingCookieManager, Microsoft.AspNetCore.Authentication.Cookies.ICookieManager
    {
        private readonly ISbnContextAccessor _sbnContextAccessor;
        private readonly IRuntimeState _runtime;
        private readonly string[] _explicitPaths;
        private readonly SbnRequestPaths _sbnRequestPaths;
        private readonly IBasicAuthService _basicAuthService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BackOfficeCookieManager"/> class.
        /// </summary>
        public BackOfficeCookieManager(
            ISbnContextAccessor sbnContextAccessor,
            IRuntimeState runtime,
            SbnRequestPaths sbnRequestPaths,
            IBasicAuthService basicAuthService)
            : this(sbnContextAccessor, runtime, null, sbnRequestPaths, basicAuthService)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BackOfficeCookieManager"/> class.
        /// </summary>
        public BackOfficeCookieManager(
            ISbnContextAccessor sbnContextAccessor,
            IRuntimeState runtime,
            IEnumerable<string> explicitPaths,
            SbnRequestPaths sbnRequestPaths,
            IBasicAuthService basicAuthService)
        {
            _sbnContextAccessor = sbnContextAccessor;
            _runtime = runtime;
            _explicitPaths = explicitPaths?.ToArray();
            _sbnRequestPaths = sbnRequestPaths;
            _basicAuthService = basicAuthService;
        }

        /// <summary>
        /// Determines if we should authenticate the request
        /// </summary>
        /// <returns>true if the request should be authenticated</returns>
        /// <remarks>
        /// We auth the request when:
        /// * it is a back office request
        /// * it is an installer request
        /// * it is a preview request
        /// </remarks>
        public bool ShouldAuthenticateRequest(string absPath)
        {
            // Do not authenticate the request if we are not running (don't have a db, are not configured) - since we will never need
            // to know a current user in this scenario - we treat it as a new install. Without this we can have some issues
            // when people have older invalid cookies on the same domain since our user managers might attempt to lookup a user
            // and we don't even have a db.
            // was: app.IsConfigured == false (equiv to !Run) && dbContext.IsDbConfigured == false (equiv to Install)
            // so, we handle .Install here and NOT .Upgrade
            if (_runtime.Level == RuntimeLevel.Install)
            {
                return false;
            }

            // check the explicit paths
            if (_explicitPaths != null)
            {
                return _explicitPaths.Any(x => x.InvariantEquals(absPath));
            }

            if (// check back office
                _sbnRequestPaths.IsBackOfficeRequest(absPath)

                // check installer
                || _sbnRequestPaths.IsInstallerRequest(absPath))
            {
                return true;
            }

            if (_basicAuthService.IsBasicAuthEnabled())
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Explicitly implement this so that we filter the request
        /// </summary>
        /// <inheritdoc/>
        string Microsoft.AspNetCore.Authentication.Cookies.ICookieManager.GetRequestCookie(HttpContext context, string key)
        {
            var absPath = context.Request.Path;
            if (!_sbnContextAccessor.TryGetSbnContext(out _) || _sbnRequestPaths.IsClientSideRequest(absPath))
            {
                return null;
            }

            return ShouldAuthenticateRequest(absPath) == false

                // Don't auth request, don't return a cookie
                ? null

                // Return the default implementation
                : GetRequestCookie(context, key);
        }

    }
}
