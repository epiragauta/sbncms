using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sbn.Cms.Core.Cache;
using Sbn.Cms.Core.Logging;
using Sbn.Cms.Core.Routing;
using Sbn.Cms.Core.Security;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Web;
using Sbn.Cms.Infrastructure.Persistence;
using Sbn.Cms.Web.Common.Filters;
using Sbn.Cms.Web.Common.Models;
using Sbn.Cms.Web.Common.Security;
using Sbn.Extensions;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Sbn.Cms.Web.Website.Controllers
{
    public class UmbLoginController : SurfaceController
    {
        private readonly IMemberSignInManager _signInManager;

        public UmbLoginController(
            ISbnContextAccessor sbnContextAccessor,
            ISbnDatabaseFactory databaseFactory,
            ServiceContext services,
            AppCaches appCaches,
            IProfilingLogger profilingLogger,
            IPublishedUrlProvider publishedUrlProvider,
            IMemberSignInManager signInManager)
            : base(sbnContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            _signInManager = signInManager;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateSbnFormRouteString]
        public async Task<IActionResult> HandleLogin([Bind(Prefix = "loginModel")]LoginModel model)
        {
            if (ModelState.IsValid == false)
            {
                return CurrentSbnPage();
            }

            MergeRouteValuesToModel(model);

            // Sign the user in with username/password, this also gives a chance for developers to
            // custom verify the credentials and auto-link user accounts with a custom IBackOfficePasswordChecker
            SignInResult result = await _signInManager.PasswordSignInAsync(
                model.Username, model.Password, isPersistent: model.RememberMe, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                TempData["LoginSuccess"] = true;

                // If there is a specified path to redirect to then use it.
                if (model.RedirectUrl.IsNullOrWhiteSpace() == false)
                {
                    // Validate the redirect URL.
                    // If it's not a local URL we'll redirect to the root of the current site.
                    return Redirect(Url.IsLocalUrl(model.RedirectUrl)
                        ? model.RedirectUrl
                        : CurrentPage.AncestorOrSelf(1).Url(PublishedUrlProvider));
                }

                // Redirect to current URL by default.
                // This is different from the current 'page' because when using Public Access the current page
                // will be the login page, but the URL will be on the requested page so that's where we need
                // to redirect too.
                return RedirectToCurrentSbnUrl();
            }

            if (result.RequiresTwoFactor)
            {
                throw new NotImplementedException("Two factor support is not supported for Sbn members yet");
            }

            // TODO: We can check for these and respond differently if we think it's important
            //  result.IsLockedOut
            //  result.IsNotAllowed

            // Don't add a field level error, just model level.
            ModelState.AddModelError("loginModel", "Invalid username or password");
            return CurrentSbnPage();
        }

        /// <summary>
        /// We pass in values via encrypted route values so they cannot be tampered with and merge them into the model for use
        /// </summary>
        /// <param name="model"></param>
        private void MergeRouteValuesToModel(LoginModel model)
        {
            if (RouteData.Values.TryGetValue(nameof(LoginModel.RedirectUrl), out var redirectUrl) && redirectUrl != null)
            {
                model.RedirectUrl = redirectUrl.ToString();
            }
        }
    }
}
