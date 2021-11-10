using Sbn.Cms.Core.Composing;

namespace Sbn.Cms.Core.Routing
{
    public class MediaUrlProviderCollectionBuilder : OrderedCollectionBuilderBase<MediaUrlProviderCollectionBuilder, MediaUrlProviderCollection, IMediaUrlProvider>
    {
        protected override MediaUrlProviderCollectionBuilder This => this;
    }
}
