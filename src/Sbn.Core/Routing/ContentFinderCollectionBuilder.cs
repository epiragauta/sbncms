using Sbn.Cms.Core.Composing;

namespace Sbn.Cms.Core.Routing
{
    public class ContentFinderCollectionBuilder : OrderedCollectionBuilderBase<ContentFinderCollectionBuilder, ContentFinderCollection, IContentFinder>
    {
        protected override ContentFinderCollectionBuilder This => this;
    }
}
