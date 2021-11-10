using System;
using System.Collections.Generic;
using Sbn.Cms.Core.Composing;

namespace Sbn.Cms.Core.Routing
{
    public class UrlProviderCollection : BuilderCollectionBase<IUrlProvider>
    {
        public UrlProviderCollection(Func<IEnumerable<IUrlProvider>> items) : base(items)
        {
        }
    }
}
