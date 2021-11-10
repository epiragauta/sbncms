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
    /// Provides URLs using the <c>sbnUrlAlias</c> property.
    /// </summary>
    public class AliasUrlProvider : IUrlProvider
    {
        private readonly RequestHandlerSettings _requestConfig;
        private readonly ISiteDomainMapper _siteDomainMapper;
        private readonly ISbnContextAccessor _sbnContextAccessor;
        private readonly UriUtility _uriUtility;
        private readonly IPublishedValueFallback _publishedValueFallback;

        public AliasUrlProvider(IOptions<RequestHandlerSettings> requestConfig, ISiteDomainMapper siteDomainMapper, UriUtility uriUtility, IPublishedValueFallback publishedValueFallback, ISbnContextAccessor sbnContextAccessor)
        {
            _requestConfig = requestConfig.Value;
            _siteDomainMapper = siteDomainMapper;
            _uriUtility = uriUtility;
            _publishedValueFallback = publishedValueFallback;
            _sbnContextAccessor = sbnContextAccessor;
        }

        // note - at the moment we seem to accept pretty much anything as an alias
        // without any form of validation ... could even prob. kill the XPath ...
        // ok, this is somewhat experimental and is NOT enabled by default

        #region GetUrl

        /// <inheritdoc />
        public UrlInfo GetUrl(IPublishedContent content, UrlMode mode, string culture, Uri current)
        {
            return null; // we have nothing to say
        }

        #endregion

        #region GetOtherUrls

        /// <summary>
        /// Gets the other URLs of a published content.
        /// </summary>
        /// <param name="sbnContext">The Sbn context.</param>
        /// <param name="id">The published content id.</param>
        /// <param name="current">The current absolute URL.</param>
        /// <returns>The other URLs for the published content.</returns>
        /// <remarks>
        /// <para>Other URLs are those that <c>GetUrl</c> would not return in the current context, but would be valid
        /// URLs for the node in other contexts (different domain for current request, sbnUrlAlias...).</para>
        /// </remarks>
        public IEnumerable<UrlInfo> GetOtherUrls(int id, Uri current)
        {
            var sbnContext = _sbnContextAccessor.GetRequiredSbnContext();
            var node = sbnContext.Content.GetById(id);
            if (node == null)
                yield break;

            if (!node.HasProperty(Constants.Conventions.Content.UrlAlias))
                yield break;

            // look for domains, walking up the tree
            var n = node;
            var domainUris = DomainUtilities.DomainsForNode(sbnContext.PublishedSnapshot.Domains, _siteDomainMapper, n.Id, current, false);
            while (domainUris == null && n != null) // n is null at root
            {
                // move to parent node
                n = n.Parent;
                domainUris = n == null ? null : DomainUtilities.DomainsForNode(sbnContext.PublishedSnapshot.Domains, _siteDomainMapper, n.Id, current, excludeDefault: false);
            }

            // determine whether the alias property varies
            var varies = node.GetProperty(Constants.Conventions.Content.UrlAlias).PropertyType.VariesByCulture();

            if (domainUris == null)
            {
                // no domain
                // if the property is invariant, then URL "/<alias>" is ok
                // if the property varies, then what are we supposed to do?
                //  the content finder may work, depending on the 'current' culture,
                //  but there's no way we can return something meaningful here
                if (varies)
                    yield break;

                var sbnUrlName = node.Value<string>(_publishedValueFallback, Constants.Conventions.Content.UrlAlias);
                var aliases = sbnUrlName?.Split(Constants.CharArrays.Comma, StringSplitOptions.RemoveEmptyEntries);

                if (aliases == null || aliases.Any() == false)
                    yield break;

                foreach (var alias in aliases.Distinct())
                {
                    var path = "/" + alias;
                    var uri = new Uri(path, UriKind.Relative);
                    yield return UrlInfo.Url(_uriUtility.UriFromSbn(uri, _requestConfig).ToString());
                }
            }
            else
            {
                // some domains: one URL per domain, which is "<domain>/<alias>"
                foreach (var domainUri in domainUris)
                {
                    // if the property is invariant, get the invariant value, URL is "<domain>/<invariant-alias>"
                    // if the property varies, get the variant value, URL is "<domain>/<variant-alias>"

                    // but! only if the culture is published, else ignore
                    if (varies && !node.HasCulture(domainUri.Culture)) continue;

                    var sbnUrlName = varies
                        ? node.Value<string>(_publishedValueFallback,Constants.Conventions.Content.UrlAlias, culture: domainUri.Culture)
                        : node.Value<string>(_publishedValueFallback, Constants.Conventions.Content.UrlAlias);

                    var aliases = sbnUrlName?.Split(Constants.CharArrays.Comma, StringSplitOptions.RemoveEmptyEntries);

                    if (aliases == null || aliases.Any() == false)
                        continue;

                    foreach(var alias in aliases.Distinct())
                    {
                        var path = "/" + alias;
                        var uri = new Uri(CombinePaths(domainUri.Uri.GetLeftPart(UriPartial.Path), path));
                        yield return UrlInfo.Url(_uriUtility.UriFromSbn(uri, _requestConfig).ToString(), domainUri.Culture);
                    }
                }
            }
        }

        #endregion

        #region Utilities

        string CombinePaths(string path1, string path2)
        {
            string path = path1.TrimEnd(Constants.CharArrays.ForwardSlash) + path2;
            return path == "/" ? path : path.TrimEnd(Constants.CharArrays.ForwardSlash);
        }

        #endregion
    }
}
