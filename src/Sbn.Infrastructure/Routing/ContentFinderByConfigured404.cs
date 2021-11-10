using System;
using System.Globalization;
using System.Linq;
using Examine;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.Models.PublishedContent;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Web;
using Sbn.Cms.Infrastructure;
using Sbn.Extensions;

namespace Sbn.Cms.Core.Routing
{
    /// <summary>
    /// Provides an implementation of <see cref="IContentFinder"/> that runs the legacy 404 logic.
    /// </summary>
    public class ContentFinderByConfigured404 : IContentLastChanceFinder
    {
        private readonly ILogger<ContentFinderByConfigured404> _logger;
        private readonly IEntityService _entityService;
        private readonly ContentSettings _contentSettings;
        private readonly IExamineManager _examineManager;
        private readonly ISbnContextAccessor _sbnContextAccessor;
        private readonly IVariationContextAccessor _variationContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentFinderByConfigured404"/> class.
        /// </summary>
        public ContentFinderByConfigured404(
            ILogger<ContentFinderByConfigured404> logger,
            IEntityService entityService,
            IOptions<ContentSettings> contentConfigSettings,
            IExamineManager examineManager,
            IVariationContextAccessor variationContextAccessor,
            ISbnContextAccessor sbnContextAccessor)
        {
            _logger = logger;
            _entityService = entityService;
            _contentSettings = contentConfigSettings.Value;
            _examineManager = examineManager;
            _variationContextAccessor = variationContextAccessor;
            _sbnContextAccessor = sbnContextAccessor;
        }

        /// <summary>
        /// Tries to find and assign an Sbn document to a <c>PublishedRequest</c>.
        /// </summary>
        /// <param name="frequest">The <c>PublishedRequest</c>.</param>
        /// <returns>A value indicating whether an Sbn document was found and assigned.</returns>
        public bool TryFindContent(IPublishedRequestBuilder frequest)
        {
            if (!_sbnContextAccessor.TryGetSbnContext(out var sbnContext))
            {
                return false;
            }
            _logger.LogDebug("Looking for a page to handle 404.");

            int? domainContentId = null;

            // try to find a culture as best as we can
            string errorCulture = CultureInfo.CurrentUICulture.Name;
            if (frequest.Domain != null)
            {
                errorCulture = frequest.Domain.Culture;
                domainContentId = frequest.Domain.ContentId;
            }
            else
            {
                var route = frequest.AbsolutePathDecoded;
                var pos = route.LastIndexOf('/');
                IPublishedContent node = null;
                while (pos > 1)
                {
                    route = route.Substring(0, pos);
                    node = sbnContext.Content.GetByRoute(route, culture: frequest?.Culture);
                    if (node != null)
                    {
                        break;
                    }

                    pos = route.LastIndexOf('/');
                }

                if (node != null)
                {
                    Domain d = DomainUtilities.FindWildcardDomainInPath(sbnContext.PublishedSnapshot.Domains.GetAll(true), node.Path, null);
                    if (d != null)
                    {
                        errorCulture = d.Culture;
                    }
                }
            }

            var error404 = NotFoundHandlerHelper.GetCurrentNotFoundPageId(
                _contentSettings.Error404Collection.ToArray(),
                _entityService,
                new PublishedContentQuery(sbnContext.PublishedSnapshot, _variationContextAccessor, _examineManager),
                errorCulture,
                domainContentId);

            IPublishedContent content = null;

            if (error404.HasValue)
            {
                _logger.LogDebug("Got id={ErrorNodeId}.", error404.Value);

                content = sbnContext.Content.GetById(error404.Value);

                _logger.LogDebug(content == null
                    ? "Could not find content with that id."
                    : "Found corresponding content.");
            }
            else
            {
                _logger.LogDebug("Got nothing.");
            }

            frequest
                .SetPublishedContent(content)
                .SetIs404();

            return content != null;
        }
    }
}
