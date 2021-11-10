using System.Collections.Generic;

namespace Sbn.Cms.Core.Configuration.Grid
{
    public interface IGridEditorsConfig
    {
        IEnumerable<IGridEditorConfig> Editors { get; }
    }
}
