// Copyright (c) Sbn.
// See LICENSE for more details.

using Sbn.Cms.Core.IO;

namespace Sbn.Cms.Core.PropertyEditors
{
    /// <summary>
    /// Represents the configuration editor for the textarea value editor.
    /// </summary>
    public class TextAreaConfigurationEditor : ConfigurationEditor<TextAreaConfiguration>
    {
        public TextAreaConfigurationEditor(IIOHelper ioHelper) : base(ioHelper)
        {
        }
    }
}
