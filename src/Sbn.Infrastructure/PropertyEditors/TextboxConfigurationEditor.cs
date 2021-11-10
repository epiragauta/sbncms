// Copyright (c) Sbn.
// See LICENSE for more details.

using Sbn.Cms.Core.IO;

namespace Sbn.Cms.Core.PropertyEditors
{
    /// <summary>
    /// Represents the configuration editor for the textbox value editor.
    /// </summary>
    public class TextboxConfigurationEditor : ConfigurationEditor<TextboxConfiguration>
    {
        public TextboxConfigurationEditor(IIOHelper ioHelper) : base(ioHelper)
        {
        }
    }
}
