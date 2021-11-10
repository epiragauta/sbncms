using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sbn.Cms.Core.Cache;
using Sbn.Cms.Core.Logging;
using Sbn.Cms.Core.Routing;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Web;
using Sbn.Cms.Infrastructure.Persistence;
using Sbn.Cms.Web.Common.Filters;
using Sbn.Cms.Web.Common.Models;
using Sbn.Cms.Web.Common.Security;
using Sbn.Extensions;

namespace Sbn.Cms.Web.Website.Controllers
{
    [SbnMemberAuthorize]
    public class UmbLoginStatusController : SurfaceController
    {
        private readonly IMemberSignInManager _signInManager;

        public UmbLoginStatusController(
            ISbnContextAccessor sbnContextAccessor,
            ISbnDatabaseFactory databaseFactory,
            ServiceContext services,
            AppCaches appCaches,
            IProfilingLogger profilingLogger,
            IPublishedUrlProvider publishedUrlProvider,
            IMemberSignInManager signInManager)
            : base(sbnContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
            => _signInManager = signInManager;

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateSbnFormRouteString]
        public async Task<IActionResult> HandleLogout([Bind(Prefix = "logoutModel")]PostRedirectModel model)
        {
            if (ModelState.IsValid == false)
            {
                return CurrentSbnPage();
            }

            var isLoggedIn = HttpContext.User?.Identity?.IsAuthenticated ?? false;

            if (isLoggedIn)
            {
                await _signInManager.SignOutAsync();
            }

            TempData["LogoutSuccess"] = true;

            // If there is a specified path to redirect to then use it.
            if (model.RedirectUrl.IsNullOrWhiteSpace() == false)
            {
                return Redirect(model.RedirectUrl);
            }

            // Redirect to current page by default.
            return RedirectToCurrentSbnPage();
        }
    }
}
