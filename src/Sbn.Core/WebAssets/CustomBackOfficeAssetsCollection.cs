using System;
using System.Collections.Generic;
using Sbn.Cms.Core.Composing;

namespace Sbn.Cms.Core.WebAssets
{
    public class CustomBackOfficeAssetsCollection : BuilderCollectionBase<IAssetFile>
    {
        public CustomBackOfficeAssetsCollection(Func<IEnumerable<IAssetFile>> items) : base(items)
        {
        }
    }
}
