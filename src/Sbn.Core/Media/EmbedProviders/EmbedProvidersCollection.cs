using System;
using System.Collections.Generic;
using Sbn.Cms.Core.Composing;

namespace Sbn.Cms.Core.Media.EmbedProviders
{
    public class EmbedProvidersCollection : BuilderCollectionBase<IEmbedProvider>
    {
        public EmbedProvidersCollection(Func<IEnumerable<IEmbedProvider>> items) : base(items)
        {
        }
    }
}
