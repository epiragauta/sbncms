using Microsoft.Extensions.Options;
using Sbn.Cms.Web.Common.Security;

namespace Sbn.Cms.Web.BackOffice.Security
{
    /// <summary>
    /// Configures the back office security stamp options
    /// </summary>
    public class ConfigureBackOfficeSecurityStampValidatorOptions : IConfigureOptions<BackOfficeSecurityStampValidatorOptions>
    {
        public void Configure(BackOfficeSecurityStampValidatorOptions options)
            => ConfigureSecurityStampOptions.ConfigureOptions(options);
    }


}
