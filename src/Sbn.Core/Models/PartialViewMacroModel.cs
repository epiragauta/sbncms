using System.Collections.Generic;
using Sbn.Cms.Core.Models.PublishedContent;

namespace Sbn.Cms.Core.Models
{
    /// <summary>
    /// The model used when rendering Partial View Macros
    /// </summary>
    public class PartialViewMacroModel : IContentModel
    {

        public PartialViewMacroModel(IPublishedContent page,
            int macroId,
            string macroAlias,
            string macroName,
            IDictionary<string, object> macroParams)
        {
            Content = page;
            MacroParameters = macroParams;
            MacroName = macroName;
            MacroAlias = macroAlias;
            MacroId = macroId;
        }

        public IPublishedContent Content { get; }
        public string MacroName { get; }
        public string MacroAlias { get; }
        public int MacroId { get; }
        public IDictionary<string, object> MacroParameters { get; }
    }
}
