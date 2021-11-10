using System;
using System.Collections.Generic;
using Sbn.Cms.Core.Composing;

namespace Sbn.Cms.Core.Routing
{
    public class MediaUrlProviderCollection : BuilderCollectionBase<IMediaUrlProvider>
    {
        public MediaUrlProviderCollection(Func<IEnumerable<IMediaUrlProvider>> items) : base(items)
        {
        }
    }
}
