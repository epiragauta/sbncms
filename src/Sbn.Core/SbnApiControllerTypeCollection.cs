using System;
using System.Collections.Generic;
using Sbn.Cms.Core.Composing;

namespace Sbn.Cms.Core
{
    public class SbnApiControllerTypeCollection : BuilderCollectionBase<Type>
    {
        public SbnApiControllerTypeCollection(Func<IEnumerable<Type>> items) : base(items)
        {
        }
    }
}
