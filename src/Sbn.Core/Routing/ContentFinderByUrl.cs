using Microsoft.Extensions.Logging;
using Sbn.Cms.Core.Models.PublishedContent;
using Sbn.Cms.Core.Web;

namespace Sbn.Cms.Core.Routing
{
    /// <summary>
    /// Provides an implementation of <see cref="IContentFinder"/> that handles page nice URLs.
    /// </summary>
    /// <remarks>
    /// <para>Handles <c>/foo/bar</c> where <c>/foo/bar</c> is the nice URL of a document.</para>
    /// </remarks>
    public class ContentFinderByUrl : IContentFinder
    {
        private readonly ILogger<ContentFinderByUrl> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentFinderByUrl"/> class.
        /// </summary>
        public ContentFinderByUrl(ILogger<ContentFinderByUrl> logger, ISbnContextAccessor sbnContextAccessor)
        {
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            SbnContextAccessor = sbnContextAccessor ?? throw new System.ArgumentNullException(nameof(sbnContextAccessor));
        }

        /// <summary>
        /// Gets the <see cref="ISbnContextAccessor"/>
        /// </summary>
        protected ISbnContextAccessor SbnContextAccessor { get; }

        /// <summary>
        /// Tries to find and assign an Sbn document to a <c>PublishedRequest</c>.
        /// </summary>
        /// <param name="frequest">The <c>PublishedRequest</c>.</param>
        /// <returns>A value indicating whether an Sbn document was found and assigned.</returns>
        public virtual bool TryFindContent(IPublishedRequestBuilder frequest)
        {
            if (!SbnContextAccessor.TryGetSbnContext(out var sbnContext))
            {
                return false;
            }

            string route;
            if (frequest.Domain != null)
            {
                route = frequest.Domain.ContentId + DomainUtilities.PathRelativeToDomain(frequest.Domain.Uri, frequest.AbsolutePathDecoded);
            }
            else
            {
                route = frequest.AbsolutePathDecoded;
            }

            IPublishedContent node = FindContent(frequest, route);
            return node != null;
        }

        /// <summary>
        /// Tries to find an Sbn document for a <c>PublishedRequest</c> and a route.
        /// </summary>
        /// <returns>The document node, or null.</returns>
        protected IPublishedContent FindContent(IPublishedRequestBuilder docreq, string route)
        {
            if (!SbnContextAccessor.TryGetSbnContext(out var sbnContext))
            {
                return null;
            }

            if (docreq == null)
            {
                throw new System.ArgumentNullException(nameof(docreq));
            }

            _logger.LogDebug("Test route {Route}", route);

            IPublishedContent node = sbnContext.Content.GetByRoute(sbnContext.InPreviewMode, route, culture: docreq.Culture);
            if (node != null)
            {
                docreq.SetPublishedContent(node);
                _logger.LogDebug("Got content, id={NodeId}", node.Id);
            }
            else
            {
                _logger.LogDebug("No match.");
            }

            return node;
        }
    }
}
