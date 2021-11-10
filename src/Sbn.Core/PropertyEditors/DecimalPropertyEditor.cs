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
    /// Represents a decimal property and parameter editor.
    /// </summary>
    [DataEditor(
        Constants.PropertyEditors.Aliases.Decimal,
        EditorType.PropertyValue | EditorType.MacroParameter,
        "Decimal",
        "decimal",
        ValueType = ValueTypes.Decimal)]
    public class DecimalPropertyEditor : DataEditor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DecimalPropertyEditor"/> class.
        /// </summary>
        public DecimalPropertyEditor(
            IDataValueEditorFactory dataValueEditorFactory)
            : base(dataValueEditorFactory)
        { }

        /// <inheritdoc />
        protected override IDataValueEditor CreateValueEditor()
        {
            var editor = base.CreateValueEditor();
            editor.Validators.Add(new DecimalValidator());
            return editor;
        }

        /// <inheritdoc />
        protected override IConfigurationEditor CreateConfigurationEditor() => new DecimalConfigurationEditor();
    }
}
