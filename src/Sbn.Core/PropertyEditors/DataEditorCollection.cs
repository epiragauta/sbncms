using System;
using System.Collections.Generic;
using Sbn.Cms.Core.Composing;

namespace Sbn.Cms.Core.PropertyEditors
{
    public class DataEditorCollection : BuilderCollectionBase<IDataEditor>
    {
        public DataEditorCollection(Func<IEnumerable<IDataEditor>> items) : base(items)
        {
        }
    }
}
