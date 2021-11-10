using System;
using Sbn.Cms.Core.Security;
using Sbn.Cms.Infrastructure.Persistence.Dtos;

namespace Sbn.Cms.Infrastructure.Persistence.Mappers
{
    [MapperFor(typeof(IIdentityUserLogin))]
    [MapperFor(typeof(IdentityUserLogin))]
    public sealed class ExternalLoginMapper : BaseMapper
    {
        public ExternalLoginMapper(Lazy<ISqlContext> sqlContext, MapperConfigurationStore maps)
            : base(sqlContext, maps)
        { }

        protected override void DefineMaps()
        {
            DefineMap<IdentityUserLogin, ExternalLoginDto>(nameof(IdentityUserLogin.Id), nameof(ExternalLoginDto.Id));
            DefineMap<IdentityUserLogin, ExternalLoginDto>(nameof(IdentityUserLogin.CreateDate), nameof(ExternalLoginDto.CreateDate));
            DefineMap<IdentityUserLogin, ExternalLoginDto>(nameof(IdentityUserLogin.LoginProvider), nameof(ExternalLoginDto.LoginProvider));
            DefineMap<IdentityUserLogin, ExternalLoginDto>(nameof(IdentityUserLogin.ProviderKey), nameof(ExternalLoginDto.ProviderKey));
            DefineMap<IdentityUserLogin, ExternalLoginDto>(nameof(IdentityUserLogin.UserId), nameof(ExternalLoginDto.UserId));
        }
    }
}
