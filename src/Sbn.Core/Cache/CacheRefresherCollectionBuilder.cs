using Sbn.Cms.Core.Composing;

namespace Sbn.Cms.Core.Cache
{
    public class CacheRefresherCollectionBuilder : LazyCollectionBuilderBase<CacheRefresherCollectionBuilder, CacheRefresherCollection, ICacheRefresher>
    {
        protected override CacheRefresherCollectionBuilder This => this;
    }
}
