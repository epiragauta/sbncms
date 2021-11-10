using Sbn.Cms.Core.Composing;

namespace Sbn.Cms.Core.Routing
{
    public class UrlProviderCollectionBuilder : OrderedCollectionBuilderBase<UrlProviderCollectionBuilder, UrlProviderCollection, IUrlProvider>
    {
        protected override UrlProviderCollectionBuilder This => this;
    }
}
