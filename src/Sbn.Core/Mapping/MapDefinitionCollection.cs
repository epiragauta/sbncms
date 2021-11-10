using System;
using System.Collections.Generic;
using Sbn.Cms.Core.Composing;

namespace Sbn.Cms.Core.Mapping
{
    public class MapDefinitionCollection : BuilderCollectionBase<IMapDefinition>
    {
        public MapDefinitionCollection(Func<IEnumerable<IMapDefinition>> items) : base(items)
        {
        }
    }
}
