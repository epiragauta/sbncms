using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Sbn.Cms.Core.Configuration;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Security;
using Sbn.Cms.Core.Semver;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Web;
using Sbn.Cms.Web.Common.Attributes;
using Sbn.Extensions;
using Constants = Sbn.Cms.Core.Constants;

namespace Sbn.Cms.Web.BackOffice.Controllers
{
    [PluginController(Constants.Web.Mvc.BackOfficeApiArea)]
    public class UpdateCheckController : SbnAuthorizedJsonController
    {
        private readonly IUpgradeService _upgradeService;
        private readonly ISbnVersion _sbnVersion;
        private readonly ICookieManager _cookieManager;
        private readonly IBackOfficeSecurityAccessor _backofficeSecurityAccessor;
        private readonly GlobalSettings _globalSettings;

        public UpdateCheckController(
            IUpgradeService upgradeService,
            ISbnVersion sbnVersion,
            ICookieManager cookieManager,
            IBackOfficeSecurityAccessor backofficeSecurityAccessor,
            IOptions<GlobalSettings> globalSettings)
        {
            _upgradeService = upgradeService ?? throw new ArgumentNullException(nameof(upgradeService));
            _sbnVersion = sbnVersion ?? throw new ArgumentNullException(nameof(sbnVersion));
            _cookieManager = cookieManager ?? throw new ArgumentNullException(nameof(cookieManager));
            _backofficeSecurityAccessor = backofficeSecurityAccessor ?? throw new ArgumentNullException(nameof(backofficeSecurityAccessor));
            _globalSettings = globalSettings.Value ?? throw new ArgumentNullException(nameof(globalSettings));
        }

        [UpdateCheckResponseFilter]
        public async Task<UpgradeCheckResponse> GetCheck()
        {
            var updChkCookie = _cookieManager.GetCookieValue("UMB_UPDCHK");
            var updateCheckCookie = updChkCookie ?? string.Empty;
            if (_globalSettings.VersionCheckPeriod > 0 && string.IsNullOrEmpty(updateCheckCookie) && _backofficeSecurityAccessor.BackOfficeSecurity.CurrentUser.IsAdmin())
            {
                try
                {
                    var version = new SemVersion(_sbnVersion.Version.Major, _sbnVersion.Version.Minor,
                        _sbnVersion.Version.Build, _sbnVersion.Comment);
                    var result = await _upgradeService.CheckUpgrade(version);

                    return new UpgradeCheckResponse(result.UpgradeType, result.Comment, result.UpgradeUrl, _sbnVersion);
                }
                catch
                {
                    //We don't want to crash due to this
                    return null;
                }
            }
            return null;
        }

        /// <summary>
        /// Adds the cookie response if it was successful
        /// </summary>
        /// <remarks>
        /// A filter is required because we are returning an object from the get method and not an HttpResponseMessage
        /// </remarks>
        ///
        internal class UpdateCheckResponseFilterAttribute : TypeFilterAttribute
        {
            public UpdateCheckResponseFilterAttribute() : base(typeof(UpdateCheckResponseFilter))
            {
            }

            private class UpdateCheckResponseFilter : IActionFilter
            {
                private readonly GlobalSettings _globalSettings;

                public UpdateCheckResponseFilter(IOptions<GlobalSettings> globalSettings)
                {
                    _globalSettings = globalSettings.Value;
                }

                public void OnActionExecuted(ActionExecutedContext context)
                {
                    if (context.HttpContext.Response == null) return;

                    if (context.Result is ObjectResult objectContent)
                    {
                        if (objectContent.Value == null) return;

                        context.HttpContext.Response.Cookies.Append("UMB_UPDCHK", "1", new CookieOptions()
                        {
                            Path = "/",
                            Expires = DateTimeOffset.Now.AddDays(_globalSettings.VersionCheckPeriod),
                            HttpOnly = true,
                            Secure = _globalSettings.UseHttps
                        });
                    }
                }

                public void OnActionExecuting(ActionExecutingContext context)
                {

                }
            }
        }

    }
}
