using Sbn.Cms.Infrastructure;

namespace Sbn.Cms.Core
{
    public interface IPublishedContentQueryAccessor
    {
        bool TryGetValue(out IPublishedContentQuery publishedContentQuery);
    }
}
