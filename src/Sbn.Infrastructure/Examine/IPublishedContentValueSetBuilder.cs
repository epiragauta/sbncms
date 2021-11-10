using Examine;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Infrastructure.Examine
{
    /// <summary>
    /// Marker interface for a <see cref="ValueSet"/> builder for only published content
    /// </summary>
    public interface IPublishedContentValueSetBuilder : IValueSetBuilder<IContent>
    {
    }
}
