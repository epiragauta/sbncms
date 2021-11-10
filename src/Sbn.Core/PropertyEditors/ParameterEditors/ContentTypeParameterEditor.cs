using Microsoft.Extensions.Logging;
using Sbn.Cms.Core.Hosting;
using Sbn.Cms.Core.Serialization;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Strings;

namespace Sbn.Cms.Core.PropertyEditors.ParameterEditors
{
    /// <summary>
    /// Represents a content type parameter editor.
    /// </summary>
    [DataEditor(
        "contentType",
        EditorType.MacroParameter,
        "Content Type Picker",
        "entitypicker")]
    public class ContentTypeParameterEditor : DataEditor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentTypeParameterEditor"/> class.
        /// </summary>
        public ContentTypeParameterEditor(
            IDataValueEditorFactory dataValueEditorFactory)
            : base(dataValueEditorFactory)
        {
            // configure
            DefaultConfiguration.Add("multiple", false);
            DefaultConfiguration.Add("entityType", "DocumentType");
        }
    }
}
