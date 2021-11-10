// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Sbn.Extensions;

namespace Sbn.Cms.Web.Common.Localization
{
    /// <summary>
    /// Sets the request culture to the culture of the back office user if one is determined to be in the request
    /// </summary>
    public class SbnBackOfficeIdentityCultureProvider : RequestCultureProvider
    {
        private readonly RequestLocalizationOptions _localizationOptions;
        private readonly object _locker = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="SbnBackOfficeIdentityCultureProvider"/> class.
        /// </summary>
        public SbnBackOfficeIdentityCultureProvider(RequestLocalizationOptions localizationOptions) => _localizationOptions = localizationOptions;

        /// <inheritdoc/>
        public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            CultureInfo culture = httpContext.User.Identity.GetCulture();

            if (culture is null)
            {
                return NullProviderCultureResult;
            }

            lock (_locker)
            {
                // We need to dynamically change the supported cultures since we won't ever know what languages are used since
                // they are dynamic within Sbn. We have to handle this for both UI and Region cultures, in case people run different region and UI languages
                var cultureExists = _localizationOptions.SupportedCultures.Contains(culture);

                if (!cultureExists)
                {
                    // add this as a supporting culture
                    _localizationOptions.SupportedCultures.Add(culture);
                }

                var uiCultureExists = _localizationOptions.SupportedCultures.Contains(culture);

                if (!uiCultureExists)
                {
                    // add this as a supporting culture
                    _localizationOptions.SupportedUICultures.Add(culture);
                }

                return Task.FromResult(new ProviderCultureResult(culture.Name));
            }
        }
    }
}
