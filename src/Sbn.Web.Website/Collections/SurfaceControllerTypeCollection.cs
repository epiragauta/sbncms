using System;
using System.Collections.Generic;
using Sbn.Cms.Core.Composing;

namespace Sbn.Cms.Web.Website.Collections
{
    public class SurfaceControllerTypeCollection : BuilderCollectionBase<Type>
    {
        public SurfaceControllerTypeCollection(Func<IEnumerable<Type>> items) : base(items)
        {
        }
    }
}
