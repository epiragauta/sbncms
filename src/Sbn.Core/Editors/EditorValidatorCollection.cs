using System;
using System.Collections.Generic;
using Sbn.Cms.Core.Composing;

namespace Sbn.Cms.Core.Editors
{
    public class EditorValidatorCollection : BuilderCollectionBase<IEditorValidator>
    {
        public EditorValidatorCollection(Func<IEnumerable<IEditorValidator>> items) : base(items)
        {
        }
    }
}
