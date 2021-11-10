using System;
using System.Collections.Generic;
using NPoco;
using Sbn.Cms.Core.Composing;

namespace Sbn.Cms.Infrastructure.Persistence
{
    public sealed class NPocoMapperCollection : BuilderCollectionBase<IMapper>
    {
        public NPocoMapperCollection(Func<IEnumerable<IMapper>> items) : base(items)
        {
        }
    }
}
