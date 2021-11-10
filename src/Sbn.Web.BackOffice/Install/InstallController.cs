using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sbn.Cms.Core;
using Sbn.Cms.Core.Configuration;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.Hosting;
using Sbn.Cms.Core.Security;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.WebAssets;
using Sbn.Cms.Infrastructure.Install;
using Sbn.Cms.Web.Common.Filters;
using Sbn.Extensions;

namespace Sbn.Cms.Web.BackOffice.Install
{

    /// <summary>
    /// The Installation controller
    /// </summary>
    [InstallAuthorize]
    [Area(Cms.Core.Constants.Web.Mvc.InstallArea)]
    public class InstallController : Controller
    {
        private readonly IBackOfficeSecurityAccessor _backofficeSecurityAccessor;
        private readonly InstallHelper _installHelper;
        private readonly IRuntimeState _runtime;
        private readonly GlobalSettings _globalSettings;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ISbnVersion _sbnVersion;
        private readonly ILogger<InstallController> _logger;
        private readonly LinkGenerator _linkGenerator;
        private readonly IRuntimeMinifier _runtimeMinifier;

        public InstallController(
            IBackOfficeSecurityAccessor backofficeSecurityAccessor,
            InstallHelper installHelper,
            IRuntimeState runtime,
            IOptions<GlobalSettings> globalSettings,
            IRuntimeMinifier runtimeMinifier,
            IHostingEnvironment hostingEnvironment,
            ISbnVersion sbnVersion,
            ILogger<InstallController> logger,
            LinkGenerator linkGenerator)
        {
            _backofficeSecurityAccessor = backofficeSecurityAccessor;
            _installHelper = installHelper;
            _runtime = runtime;
            _globalSettings = globalSettings.Value;
            _runtimeMinifier = runtimeMinifier;
            _hostingEnvironment = hostingEnvironment;
            _sbnVersion = sbnVersion;
            _logger = logger;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        [StatusCodeResult(System.Net.HttpStatusCode.ServiceUnavailable)]
        [TypeFilter(typeof(StatusCodeResultAttribute), Arguments = new object []{System.Net.HttpStatusCode.ServiceUnavailable})]
        public async Task<ActionResult> Index()
        {
            var sbnPath = Url.GetBackOfficeUrl();

            if (_runtime.Level == RuntimeLevel.Run)
                return Redirect(sbnPath);

            // TODO: Update for package migrations
            if (_runtime.Level == RuntimeLevel.Upgrade)
            {
                // Update ClientDependency version and delete its temp directories to make sure we get fresh caches
                _runtimeMinifier.Reset();

                var authResult = await this.AuthenticateBackOfficeAsync();

                if (!authResult.Succeeded)
                {
                    return Redirect(_globalSettings.SbnPath + "/AuthorizeUpgrade?redir=" + Request.GetEncodedUrl());
                }
            }

            // gen the install base URL
            ViewData.SetInstallApiBaseUrl(_linkGenerator.GetInstallerApiUrl());

            // get the base sbn folder
            var baseFolder = _hostingEnvironment.ToAbsolute(_globalSettings.SbnPath);
            ViewData.SetSbnBaseFolder(baseFolder);

            ViewData.SetSbnVersion(_sbnVersion.SemanticVersion);

            await _installHelper.SetInstallStatusAsync(false, "");

            return View(Path.Combine(Constants.SystemDirectories.Sbn.TrimStart("~") , Cms.Core.Constants.Web.Mvc.InstallArea, nameof(Index) + ".cshtml"));
        }

        /// <summary>
        /// Used to perform the redirect to the installer when the runtime level is <see cref="RuntimeLevel.Install"/> or <see cref="RuntimeLevel.Upgrade"/>
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Redirect()
        {
            var uri = HttpContext.Request.GetEncodedUrl();

            // redirect to install
            ReportRuntime(_logger, _runtime.Level, "Sbn must install or upgrade.");

            var installUrl = $"{_linkGenerator.GetInstallerUrl()}?redir=true&url={uri}";
            return Redirect(installUrl);
        }

        private static bool _reported;
        private static RuntimeLevel _reportedLevel;

        private static void ReportRuntime(ILogger<InstallController> logger, RuntimeLevel level, string message)
        {
            if (_reported && _reportedLevel == level) return;
            _reported = true;
            _reportedLevel = level;
            logger.LogWarning(message);
        }
    }
}
