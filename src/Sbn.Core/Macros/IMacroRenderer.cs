using System.Collections.Generic;
using System.Threading.Tasks;
using Sbn.Cms.Core.Models.PublishedContent;

namespace Sbn.Cms.Core.Macros
{
    /// <summary>
    /// Renders a macro
    /// </summary>
    public interface IMacroRenderer
    {
        Task<MacroContent> RenderAsync(string macroAlias, IPublishedContent content, IDictionary<string, object> macroParams);
    }
}
