using System.Collections.Generic;

namespace Sbn.Cms.Core.Configuration.Grid
{
    public interface IGridEditorConfig
    {
        string Name { get; }
        string NameTemplate { get; }
        string Alias { get; }
        string View { get; }
        string Render { get; }
        string Icon { get; }
        IDictionary<string, object> Config { get; }
    }
}
