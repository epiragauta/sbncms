using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sbn.Cms.Core.Security;

namespace Sbn.Cms.Web.BackOffice.Security
{
    /// <summary>
    /// A security stamp validator for the back office
    /// </summary>
    public class BackOfficeSecurityStampValidator : SecurityStampValidator<BackOfficeIdentityUser>
    {
        public BackOfficeSecurityStampValidator(
            IOptions<BackOfficeSecurityStampValidatorOptions> options,
            BackOfficeSignInManager signInManager, ISystemClock clock, ILoggerFactory logger)
            : base(options, signInManager, clock, logger)
        {
        }

    }
}
