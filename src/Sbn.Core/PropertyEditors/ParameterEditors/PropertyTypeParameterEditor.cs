using Microsoft.Extensions.Logging;
using Sbn.Cms.Core.Hosting;
using Sbn.Cms.Core.Serialization;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Strings;

namespace Sbn.Cms.Core.PropertyEditors.ParameterEditors
{
    [DataEditor(
        "propertyTypePicker",
        EditorType.MacroParameter,
        "Property Type Picker",
        "entitypicker")]
    public class PropertyTypeParameterEditor : DataEditor
    {
        public PropertyTypeParameterEditor(
            IDataValueEditorFactory dataValueEditorFactory)
            : base(dataValueEditorFactory)
        {
            // configure
            DefaultConfiguration.Add("multiple", "0");
            DefaultConfiguration.Add("entityType", "PropertyType");
            //don't publish the id for a property type, publish its alias
            DefaultConfiguration.Add("publishBy", "alias");
        }
    }
}
