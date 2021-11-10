using Sbn.Cms.Core.Composing;

namespace Sbn.Cms.Core.Media.EmbedProviders
{
    public class EmbedProvidersCollectionBuilder : OrderedCollectionBuilderBase<EmbedProvidersCollectionBuilder, EmbedProvidersCollection, IEmbedProvider>
    {
        protected override EmbedProvidersCollectionBuilder This => this;
    }
}
