using System.Collections.Generic;
using Sbn.Cms.Core.Models.PublishedContent;

namespace Sbn.Cms.Infrastructure.PublishedCache.Navigable
{
    internal interface INavigableData
    {
        IPublishedContent GetById(bool preview, int contentId);
        IEnumerable<IPublishedContent> GetAtRoot(bool preview);
    }
}
