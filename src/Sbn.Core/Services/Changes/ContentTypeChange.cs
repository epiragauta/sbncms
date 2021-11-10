using System.Collections.Generic;
using System.Linq;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Services.Changes
{
    public class ContentTypeChange<TItem>
        where TItem : class, IContentTypeComposition
    {
        public ContentTypeChange(TItem item, ContentTypeChangeTypes changeTypes)
        {
            Item = item;
            ChangeTypes = changeTypes;
        }

        public TItem Item { get; }

        public ContentTypeChangeTypes ChangeTypes { get; set; }
    }

}
