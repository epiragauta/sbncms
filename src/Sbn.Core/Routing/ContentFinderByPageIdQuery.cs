using System.Globalization;
using Sbn.Cms.Core.Models.PublishedContent;
using Sbn.Cms.Core.Web;
using Sbn.Extensions;

namespace Sbn.Cms.Core.Routing
{
    /// <summary>
    /// This looks up a document by checking for the umbPageId of a request/query string
    /// </summary>
    /// <remarks>
    /// This is used by library.RenderTemplate and also some of the macro rendering functionality like in
    /// macroResultWrapper.aspx
    /// </remarks>
    public class ContentFinderByPageIdQuery : IContentFinder
    {
        private readonly IRequestAccessor _requestAccessor;
        private readonly ISbnContextAccessor _sbnContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentFinderByPageIdQuery"/> class.
        /// </summary>
        public ContentFinderByPageIdQuery(IRequestAccessor requestAccessor, ISbnContextAccessor sbnContextAccessor)
        {
            _requestAccessor = requestAccessor ?? throw new System.ArgumentNullException(nameof(requestAccessor));
            _sbnContextAccessor = sbnContextAccessor ?? throw new System.ArgumentNullException(nameof(sbnContextAccessor));
        }

        /// <inheritdoc/>
        public bool TryFindContent(IPublishedRequestBuilder frequest)
        {
            if(!_sbnContextAccessor.TryGetSbnContext(out var sbnContext))
            {
                return false;
            }
            if (int.TryParse(_requestAccessor.GetRequestValue("umbPageID"),  NumberStyles.Integer, CultureInfo.InvariantCulture, out int pageId))
            {
                IPublishedContent doc = sbnContext.Content.GetById(pageId);

                if (doc != null)
                {
                    frequest.SetPublishedContent(doc);
                    return true;
                }
            }

            return false;
        }
    }
}
