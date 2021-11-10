using Sbn.Cms.Core.Composing;

namespace Sbn.Cms.Core.Strings
{
    public class UrlSegmentProviderCollectionBuilder : OrderedCollectionBuilderBase<UrlSegmentProviderCollectionBuilder, UrlSegmentProviderCollection, IUrlSegmentProvider>
    {
        protected override UrlSegmentProviderCollectionBuilder This => this;
    }
}
