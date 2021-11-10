using System.Collections.Generic;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.PropertyEditors;

namespace Sbn.Cms.Core.Cache
{
    public interface IValueEditorCache
    {
        public IDataValueEditor GetValueEditor(IDataEditor dataEditor, IDataType dataType);
        public void ClearCache(IEnumerable<int> dataTypeIds);
    }
}
