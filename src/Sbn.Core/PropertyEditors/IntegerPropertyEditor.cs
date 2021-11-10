using Microsoft.Extensions.Logging;
using Sbn.Cms.Core.Hosting;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.PropertyEditors.Validators;
using Sbn.Cms.Core.Serialization;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Strings;

namespace Sbn.Cms.Core.PropertyEditors
{
    /// <summary>
    /// Represents an integer property and parameter editor.
    /// </summary>
    [DataEditor(
        Constants.PropertyEditors.Aliases.Integer,
        EditorType.PropertyValue | EditorType.MacroParameter,
        "Numeric",
        "integer",
        ValueType = ValueTypes.Integer)]
    public class IntegerPropertyEditor : DataEditor
    {
        public IntegerPropertyEditor(
            IDataValueEditorFactory dataValueEditorFactory)
            : base(dataValueEditorFactory)
        { }

        /// <inheritdoc />
        protected override IDataValueEditor CreateValueEditor()
        {
            var editor = base.CreateValueEditor();
            editor.Validators.Add(new IntegerValidator()); // ensure the value is validated
            return editor;
        }

        /// <inheritdoc />
        protected override IConfigurationEditor CreateConfigurationEditor() => new IntegerConfigurationEditor();
    }
}
