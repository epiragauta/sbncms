using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.Models.PublishedContent;
using Sbn.Cms.Core.Web;
using Sbn.Extensions;

namespace Sbn.Cms.Core.Routing
{
    /// <summary>
    /// Provides URLs.
    /// </summary>
    public class UrlProvider : IPublishedUrlProvider
    {
        #region Ctor and configuration

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlProvider"/> class with an Sbn context and a list of URL providers.
        /// </summary>
        /// <param name="sbnContextAccessor">The Sbn context accessor.</param>
        /// <param name="routingSettings">Routing settings.</param>
        /// <param name="urlProviders">The list of URL providers.</param>
        /// <param name="mediaUrlProviders">The list of media URL providers.</param>
        /// <param name="variationContextAccessor">The current variation accessor.</param>
        /// <param name="propertyEditorCollection"></param>
        public UrlProvider(ISbnContextAccessor sbnContextAccessor, IOptions<WebRoutingSettings> routingSettings, UrlProviderCollection urlProviders, MediaUrlProviderCollection mediaUrlProviders, IVariationContextAccessor variationContextAccessor)
        {
            _sbnContextAccessor = sbnContextAccessor ?? throw new ArgumentNullException(nameof(sbnContextAccessor));
            _urlProviders = urlProviders;
            _mediaUrlProviders = mediaUrlProviders;
            _variationContextAccessor = variationContextAccessor ?? throw new ArgumentNullException(nameof(variationContextAccessor));
            Mode = routingSettings.Value.UrlProviderMode;
        }

        private readonly ISbnContextAccessor _sbnContextAccessor;
        private readonly IEnumerable<IUrlProvider> _urlProviders;
        private readonly IEnumerable<IMediaUrlProvider> _mediaUrlProviders;
        private readonly IVariationContextAccessor _variationContextAccessor;

        /// <summary>
        /// Gets or sets the provider URL mode.
        /// </summary>
        public UrlMode Mode { get; set; }

        #endregion

        #region GetUrl

        private IPublishedContent GetDocument(int id)
        {
            var sbnContext = _sbnContextAccessor.GetRequiredSbnContext();
            return sbnContext.Content.GetById(id);
        }
        private IPublishedContent GetDocument(Guid id)
        {
            var sbnContext = _sbnContextAccessor.GetRequiredSbnContext();
            return sbnContext.Content.GetById(id);
        }
        private IPublishedContent GetMedia(Guid id)
        {
            var sbnContext = _sbnContextAccessor.GetRequiredSbnContext();
            return sbnContext.Media.GetById(id);
        }

        /// <summary>
        /// Gets the URL of a published content.
        /// </summary>
        /// <param name="id">The published content identifier.</param>
        /// <param name="mode">The URL mode.</param>
        /// <param name="culture">A culture.</param>
        /// <param name="current">The current absolute URL.</param>
        /// <returns>The URL for the published content.</returns>
        public string GetUrl(Guid id, UrlMode mode = UrlMode.Default, string culture = null, Uri current = null)
            => GetUrl(GetDocument(id), mode, culture, current);

        /// <summary>
        /// Gets the URL of a published content.
        /// </summary>
        /// <param name="id">The published content identifier.</param>
        /// <param name="mode">The URL mode.</param>
        /// <param name="culture">A culture.</param>
        /// <param name="current">The current absolute URL.</param>
        /// <returns>The URL for the published content.</returns>
        public string GetUrl(int id, UrlMode mode = UrlMode.Default, string culture = null, Uri current = null)
            => GetUrl(GetDocument(id), mode, culture, current);

        /// <summary>
        /// Gets the URL of a published content.
        /// </summary>
        /// <param name="content">The published content.</param>
        /// <param name="mode">The URL mode.</param>
        /// <param name="culture">A culture.</param>
        /// <param name="current">The current absolute URL.</param>
        /// <returns>The URL for the published content.</returns>
        /// <remarks>
        /// <para>The URL is absolute or relative depending on <c>mode</c> and on <c>current</c>.</para>
        /// <para>If the published content is multi-lingual, gets the URL for the specified culture or,
        /// when no culture is specified, the current culture.</para>
        /// <para>If the provider is unable to provide a URL, it returns "#".</para>
        /// </remarks>
        public string GetUrl(IPublishedContent content, UrlMode mode = UrlMode.Default, string culture = null, Uri current = null)
        {
            if (content == null || content.ContentType.ItemType == PublishedItemType.Element)
                return "#";

            if (mode == UrlMode.Default)
                mode = Mode;

            // this the ONLY place where we deal with default culture - IUrlProvider always receive a culture
            // be nice with tests, assume things can be null, ultimately fall back to invariant
            // (but only for variant content of course)
            if (content.ContentType.VariesByCulture())
            {
                if (culture == null)
                    culture = _variationContextAccessor?.VariationContext?.Culture ?? "";
            }

            if (current == null)
            {
                var sbnContext = _sbnContextAccessor.GetRequiredSbnContext();
                current = sbnContext.CleanedSbnUrl;
            }
            

            var url = _urlProviders.Select(provider => provider.GetUrl(content, mode, culture, current))
                .FirstOrDefault(u => u != null);
            return url?.Text ?? "#"; // legacy wants this
        }

        public string GetUrlFromRoute(int id, string route, string culture)
        {
            var sbnContext = _sbnContextAccessor.GetRequiredSbnContext();
            var provider = _urlProviders.OfType<DefaultUrlProvider>().FirstOrDefault();
            var url = provider == null
                ? route // what else?
                : provider.GetUrlFromRoute(route, sbnContext, id, sbnContext.CleanedSbnUrl, Mode, culture)?.Text;
            return url ?? "#";
        }

        #endregion

        #region GetOtherUrls

        /// <summary>
        /// Gets the other URLs of a published content.
        /// </summary>
        /// <param name="id">The published content id.</param>
        /// <returns>The other URLs for the published content.</returns>
        /// <remarks>
        /// <para>Other URLs are those that <c>GetUrl</c> would not return in the current context, but would be valid
        /// URLs for the node in other contexts (different domain for current request, sbnUrlAlias...).</para>
        /// <para>The results depend on the current URL.</para>
        /// </remarks>
        public IEnumerable<UrlInfo> GetOtherUrls(int id)
        {
            var sbnContext = _sbnContextAccessor.GetRequiredSbnContext();
            return GetOtherUrls(id, sbnContext.CleanedSbnUrl);
        }

        /// <summary>
        /// Gets the other URLs of a published content.
        /// </summary>
        /// <param name="id">The published content id.</param>
        /// <param name="current">The current absolute URL.</param>
        /// <returns>The other URLs for the published content.</returns>
        /// <remarks>
        /// <para>Other URLs are those that <c>GetUrl</c> would not return in the current context, but would be valid
        /// URLs for the node in other contexts (different domain for current request, sbnUrlAlias...).</para>
        /// </remarks>
        public IEnumerable<UrlInfo> GetOtherUrls(int id, Uri current)
        {
            return _urlProviders.SelectMany(provider => provider.GetOtherUrls(id, current) ?? Enumerable.Empty<UrlInfo>());
        }

        #endregion

        #region GetMediaUrl

        /// <summary>
        /// Gets the URL of a media item.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mode"></param>
        /// <param name="culture"></param>
        /// <param name="propertyAlias"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        public string GetMediaUrl(Guid id, UrlMode mode = UrlMode.Default, string culture = null, string propertyAlias = Constants.Conventions.Media.File, Uri current = null)
            => GetMediaUrl(GetMedia(id), mode, culture, propertyAlias, current);

        /// <summary>
        /// Gets the URL of a media item.
        /// </summary>
        /// <param name="content">The published content.</param>
        /// <param name="propertyAlias">The property alias to resolve the URL from.</param>
        /// <param name="mode">The URL mode.</param>
        /// <param name="culture">The variation language.</param>
        /// <param name="current">The current absolute URL.</param>
        /// <returns>The URL for the media.</returns>
        /// <remarks>
        /// <para>The URL is absolute or relative depending on <c>mode</c> and on <c>current</c>.</para>
        /// <para>If the media is multi-lingual, gets the URL for the specified culture or,
        /// when no culture is specified, the current culture.</para>
        /// <para>If the provider is unable to provide a URL, it returns <see cref="String.Empty"/>.</para>
        /// </remarks>
        public string GetMediaUrl(IPublishedContent content, UrlMode mode = UrlMode.Default, string culture = null, string propertyAlias = Constants.Conventions.Media.File, Uri current = null)
        {
            if (propertyAlias == null)
                throw new ArgumentNullException(nameof(propertyAlias));

            if (content == null)
                return "";

            if (mode == UrlMode.Default)
                mode = Mode;

            // this the ONLY place where we deal with default culture - IMediaUrlProvider always receive a culture
            // be nice with tests, assume things can be null, ultimately fall back to invariant
            // (but only for variant content of course)
            if (content.ContentType.VariesByCulture())
            {
                if (culture == null)
                    culture = _variationContextAccessor?.VariationContext?.Culture ?? "";
            }

            if (current == null)
            {
                var sbnContext = _sbnContextAccessor.GetRequiredSbnContext();
                current = sbnContext.CleanedSbnUrl;
            }
                

            var url = _mediaUrlProviders.Select(provider =>
                    provider.GetMediaUrl(content, propertyAlias, mode, culture, current))
                .FirstOrDefault(u => u != null);

            return url?.Text ?? "";
        }

        #endregion
    }
}
