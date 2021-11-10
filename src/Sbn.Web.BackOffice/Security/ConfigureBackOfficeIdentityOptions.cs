using System;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.Security;
using Sbn.Extensions;
using Constants = Sbn.Cms.Core.Constants;

namespace Sbn.Cms.Web.BackOffice.Security
{
    /// <summary>
    /// Used to configure <see cref="BackOfficeIdentityOptions"/> for the Sbn Back office
    /// </summary>
    public sealed class ConfigureBackOfficeIdentityOptions : IConfigureOptions<BackOfficeIdentityOptions>
    {
        private readonly UserPasswordConfigurationSettings _userPasswordConfiguration;

        public ConfigureBackOfficeIdentityOptions(IOptions<UserPasswordConfigurationSettings> userPasswordConfiguration)
        {
            _userPasswordConfiguration = userPasswordConfiguration.Value;
        }

        public void Configure(BackOfficeIdentityOptions options)
        {
            options.SignIn.RequireConfirmedAccount = true;          // uses our custom IUserConfirmation
            options.SignIn.RequireConfirmedEmail = false;           // not implemented
            options.SignIn.RequireConfirmedPhoneNumber = false;     // not implemented

            options.User.RequireUniqueEmail = true;

            options.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier;
            options.ClaimsIdentity.UserNameClaimType = ClaimTypes.Name;
            options.ClaimsIdentity.RoleClaimType = ClaimTypes.Role;
            options.ClaimsIdentity.SecurityStampClaimType = Constants.Security.SecurityStampClaimType;

            options.Lockout.AllowedForNewUsers = true;
            // TODO: Implement this
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(30);

            options.Password.ConfigurePasswordOptions(_userPasswordConfiguration);

            options.Lockout.MaxFailedAccessAttempts = _userPasswordConfiguration.MaxFailedAccessAttemptsBeforeLockout;
        }

        
    }
}
