using NPoco;
using Sbn.Cms.Core.Composing;

namespace Sbn.Cms.Infrastructure.Persistence
{
    public sealed class NPocoMapperCollectionBuilder : SetCollectionBuilderBase<NPocoMapperCollectionBuilder, NPocoMapperCollection, IMapper>
    {
        protected override NPocoMapperCollectionBuilder This => this;
    }
}
