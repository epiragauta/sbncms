using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sbn.Cms.Core.DependencyInjection;
using Sbn.Cms.Core.Security;

namespace Sbn.Extensions
{
    public static partial class SbnApplicationBuilderExtensions
    {
        public static ISbnBuilder SetBackOfficeUserManager<TUserManager>(this ISbnBuilder builder)
            where TUserManager : UserManager<BackOfficeIdentityUser>, IBackOfficeUserManager
        {

            Type customType = typeof(TUserManager);
            Type userManagerType = typeof(UserManager<BackOfficeIdentityUser>);
            builder.Services.Replace(ServiceDescriptor.Scoped(typeof(IBackOfficeUserManager), customType));
            builder.Services.AddScoped(customType, services => services.GetRequiredService(userManagerType));
            builder.Services.Replace(ServiceDescriptor.Scoped(userManagerType, customType));
            return builder;
        }
    }
}
