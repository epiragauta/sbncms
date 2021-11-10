using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Sbn.Cms.Core.Configuration.Models;

namespace Sbn.Cms.Web.Common.Localization
{
    /// <summary>
    /// Custom Sbn options configuration for <see cref="RequestLocalizationOptions"/>
    /// </summary>
    public class SbnRequestLocalizationOptions : IConfigureOptions<RequestLocalizationOptions>
    {
        private readonly IOptions<GlobalSettings> _globalSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="SbnRequestLocalizationOptions"/> class.
        /// </summary>
        public SbnRequestLocalizationOptions(IOptions<GlobalSettings> globalSettings) => _globalSettings = globalSettings;

        /// <inheritdoc/>
        public void Configure(RequestLocalizationOptions options)
        {
            // set the default culture to what is in config
            options.DefaultRequestCulture = new RequestCulture(_globalSettings.Value.DefaultUILanguage);

            // add a custom provider
            if (options.RequestCultureProviders == null)
            {
                options.RequestCultureProviders = new List<IRequestCultureProvider>();
            }

            options.RequestCultureProviders.Insert(0, new SbnBackOfficeIdentityCultureProvider(options));
            options.RequestCultureProviders.Insert(1, new SbnPublishedContentCultureProvider(options));
        }
    }
}
