// Copyright (c) Sbn.
// See LICENSE for more details.

using Microsoft.Extensions.Logging;
using Sbn.Cms.Core.IO;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Serialization;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Strings;

namespace Sbn.Cms.Core.PropertyEditors
{
    /// <summary>
    /// Represents a textbox property and parameter editor.
    /// </summary>
    [DataEditor(
        Constants.PropertyEditors.Aliases.TextBox,
        EditorType.PropertyValue | EditorType.MacroParameter,
        "Textbox",
        "textbox",
        Group = Constants.PropertyEditors.Groups.Common)]
    public class TextboxPropertyEditor : DataEditor
    {
        private readonly IIOHelper _ioHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextboxPropertyEditor"/> class.
        /// </summary>
        public TextboxPropertyEditor(
            IDataValueEditorFactory dataValueEditorFactory,
            IIOHelper ioHelper)
            : base(dataValueEditorFactory)
        {
            _ioHelper = ioHelper;
        }

        /// <inheritdoc/>
        protected override IDataValueEditor CreateValueEditor() =>
            DataValueEditorFactory.Create<TextOnlyValueEditor>(Attribute);

        /// <inheritdoc/>
        protected override IConfigurationEditor CreateConfigurationEditor() => new TextboxConfigurationEditor(_ioHelper);
    }
}
