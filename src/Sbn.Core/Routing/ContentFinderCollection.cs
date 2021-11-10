using System;
using System.Collections.Generic;
using Sbn.Cms.Core.Composing;

namespace Sbn.Cms.Core.Routing
{
    public class ContentFinderCollection : BuilderCollectionBase<IContentFinder>
    {
        public ContentFinderCollection(Func<IEnumerable<IContentFinder>> items) : base(items)
        {
        }
    }
}
