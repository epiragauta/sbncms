using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sbn.Cms.Core.Cache;
using Sbn.Cms.Core.Logging;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Routing;
using Sbn.Cms.Core.Scoping;
using Sbn.Cms.Core.Security;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Web;
using Sbn.Cms.Infrastructure.Persistence;
using Sbn.Cms.Web.Common.Filters;
using Sbn.Cms.Web.Common.Security;
using Sbn.Cms.Web.Website.Models;
using Sbn.Extensions;

namespace Sbn.Cms.Web.Website.Controllers
{
    public class UmbRegisterController : SurfaceController
    {
        private readonly IMemberManager _memberManager;
        private readonly IMemberService _memberService;
        private readonly IMemberSignInManager _memberSignInManager;
        private readonly IScopeProvider _scopeProvider;

        public UmbRegisterController(
            IMemberManager memberManager,
            IMemberService memberService,
            ISbnContextAccessor sbnContextAccessor,
            ISbnDatabaseFactory databaseFactory,
            ServiceContext services,
            AppCaches appCaches,
            IProfilingLogger profilingLogger,
            IPublishedUrlProvider publishedUrlProvider,
            IMemberSignInManager memberSignInManager,
            IScopeProvider scopeProvider)
            : base(sbnContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            _memberManager = memberManager;
            _memberService = memberService;
            _memberSignInManager = memberSignInManager;
            _scopeProvider = scopeProvider;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateSbnFormRouteString]
        public async Task<IActionResult> HandleRegisterMember([Bind(Prefix = "registerModel")]RegisterModel model)
        {
            if (ModelState.IsValid == false)
            {
                return CurrentSbnPage();
            }

            MergeRouteValuesToModel(model);

            IdentityResult result = await RegisterMemberAsync(model, true);
            if (result.Succeeded)
            {
                TempData["FormSuccess"] = true;

                // If there is a specified path to redirect to then use it.
                if (model.RedirectUrl.IsNullOrWhiteSpace() == false)
                {
                    return Redirect(model.RedirectUrl);
                }

                // Redirect to current page by default.
                return RedirectToCurrentSbnPage();
            }
            else
            {
                AddErrors(result);
                return CurrentSbnPage();
            }
        }

        /// <summary>
        /// We pass in values via encrypted route values so they cannot be tampered with and merge them into the model for use
        /// </summary>
        /// <param name="model"></param>
        private void MergeRouteValuesToModel(RegisterModel model)
        {
            if (RouteData.Values.TryGetValue(nameof(RegisterModel.RedirectUrl), out var redirectUrl) && redirectUrl != null)
            {
                model.RedirectUrl = redirectUrl.ToString();
            }

            if (RouteData.Values.TryGetValue(nameof(RegisterModel.MemberTypeAlias), out var memberTypeAlias) && memberTypeAlias != null)
            {
                model.MemberTypeAlias = memberTypeAlias.ToString();
            }

            if (RouteData.Values.TryGetValue(nameof(RegisterModel.UsernameIsEmail), out var usernameIsEmail) && usernameIsEmail != null)
            {
                model.UsernameIsEmail = usernameIsEmail.ToString() == "True";
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("registerModel", error.Description);
            }
        }

        /// <summary>
        /// Registers a new member.
        /// </summary>
        /// <param name="model">Register member model.</param>
        /// <param name="logMemberIn">Flag for whether to log the member in upon successful registration.</param>
        /// <returns>Result of registration operation.</returns>
        private async Task<IdentityResult> RegisterMemberAsync(RegisterModel model, bool logMemberIn = true)
        {
            using IScope scope = _scopeProvider.CreateScope(autoComplete: true);

            // U4-10762 Server error with "Register Member" snippet (Cannot save member with empty name)
            // If name field is empty, add the email address instead.
            if (string.IsNullOrEmpty(model.Name) && string.IsNullOrEmpty(model.Email) == false)
            {
                model.Name = model.Email;
            }

            model.Username = (model.UsernameIsEmail || model.Username == null) ? model.Email : model.Username;

            var identityUser = MemberIdentityUser.CreateNew(model.Username, model.Email, model.MemberTypeAlias, true, model.Name);
            IdentityResult identityResult = await _memberManager.CreateAsync(
                identityUser,
                model.Password);

            if (identityResult.Succeeded)
            {
                // Update the custom properties
                // TODO: See TODO in MembersIdentityUser, Should we support custom member properties for persistence/retrieval?
                IMember member = _memberService.GetByKey(identityUser.Key);
                if (member == null)
                {
                    // should never happen
                    throw new InvalidOperationException($"Could not find a member with key: {member.Key}.");
                }

                if (model.MemberProperties != null)
                {
                    foreach (MemberPropertyModel property in model.MemberProperties.Where(p => p.Value != null)
                        .Where(property => member.Properties.Contains(property.Alias)))
                    {
                        member.Properties[property.Alias].SetValue(property.Value);
                    }
                }
                _memberService.Save(member);

                if (logMemberIn)
                {
                    await _memberSignInManager.SignInAsync(identityUser, false);
                }
            }

            return identityResult;
        }
    }
}
