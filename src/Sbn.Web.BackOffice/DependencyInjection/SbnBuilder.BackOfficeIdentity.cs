using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sbn.Cms.Core.DependencyInjection;
using Sbn.Cms.Core.Net;
using Sbn.Cms.Core.Security;
using Sbn.Cms.Web.BackOffice.Security;
using Sbn.Cms.Web.Common.AspNetCore;
using Sbn.Cms.Web.Common.Security;

namespace Sbn.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="ISbnBuilder"/> for the Sbn back office
    /// </summary>
    public static partial class SbnBuilderExtensions
    {
        /// <summary>
        /// Adds Identity support for Sbn back office
        /// </summary>
        public static ISbnBuilder AddBackOfficeIdentity(this ISbnBuilder builder)
        {
            IServiceCollection services = builder.Services;

            services.AddDataProtection();

            builder.BuildSbnBackOfficeIdentity()
                .AddDefaultTokenProviders()
                .AddUserStore<BackOfficeUserStore>()
                .AddUserManager<IBackOfficeUserManager, BackOfficeUserManager>()
                .AddSignInManager<IBackOfficeSignInManager, BackOfficeSignInManager>()
                .AddClaimsPrincipalFactory<BackOfficeClaimsPrincipalFactory>()
                .AddErrorDescriber<BackOfficeErrorDescriber>();

            services.TryAddSingleton<IBackOfficeUserPasswordChecker, NoopBackOfficeUserPasswordChecker>();

            // Configure the options specifically for the SbnBackOfficeIdentityOptions instance
            services.ConfigureOptions<ConfigureBackOfficeIdentityOptions>();
            services.ConfigureOptions<ConfigureBackOfficeSecurityStampValidatorOptions>();

            return builder;
        }

        private static BackOfficeIdentityBuilder BuildSbnBackOfficeIdentity(this ISbnBuilder builder)
        {
            IServiceCollection services = builder.Services;

            services.TryAddScoped<IIpResolver, AspNetCoreIpResolver>();
            services.TryAddSingleton<IBackOfficeExternalLoginProviders, BackOfficeExternalLoginProviders>();
            services.TryAddSingleton<IBackOfficeTwoFactorOptions, NoopBackOfficeTwoFactorOptions>();

            return new BackOfficeIdentityBuilder(services);
        }

        /// <summary>
        /// Adds support for external login providers in Sbn
        /// </summary>
        public static ISbnBuilder AddBackOfficeExternalLogins(this ISbnBuilder sbnBuilder, Action<BackOfficeExternalLoginsBuilder> builder)
        {
            builder(new BackOfficeExternalLoginsBuilder(sbnBuilder.Services));
            return sbnBuilder;
        }

    }
}
