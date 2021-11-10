using System;
using Sbn.Cms.Core.Models.PublishedContent;

namespace Sbn.Cms.Core.Models
{
    /// <summary>
    /// Represents the model for the current Sbn view.
    /// </summary>
    public class ContentModel : IContentModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentModel"/> class with a content.
        /// </summary>
        public ContentModel(IPublishedContent content) => Content = content ?? throw new ArgumentNullException(nameof(content));

        /// <summary>
        /// Gets the content.
        /// </summary>
        public IPublishedContent Content { get; }
    }
}
