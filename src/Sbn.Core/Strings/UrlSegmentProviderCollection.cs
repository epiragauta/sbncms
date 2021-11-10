using System;
using System.Collections.Generic;
using Sbn.Cms.Core.Composing;

namespace Sbn.Cms.Core.Strings
{
    public class UrlSegmentProviderCollection : BuilderCollectionBase<IUrlSegmentProvider>
    {
        public UrlSegmentProviderCollection(Func<IEnumerable<IUrlSegmentProvider>> items) : base(items)
        {
        }
    }
}
