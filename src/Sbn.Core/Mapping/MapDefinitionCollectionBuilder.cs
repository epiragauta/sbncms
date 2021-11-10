using Microsoft.Extensions.DependencyInjection;
using Sbn.Cms.Core.Composing;

namespace Sbn.Cms.Core.Mapping
{
    public class MapDefinitionCollectionBuilder : SetCollectionBuilderBase<MapDefinitionCollectionBuilder, MapDefinitionCollection, IMapDefinition>
    {
        protected override MapDefinitionCollectionBuilder This => this;

        protected override ServiceLifetime CollectionLifetime => ServiceLifetime.Transient;
    }
}
