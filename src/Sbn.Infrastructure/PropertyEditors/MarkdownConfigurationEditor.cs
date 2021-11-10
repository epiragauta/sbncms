// Copyright (c) Sbn.
// See LICENSE for more details.

using Sbn.Cms.Core.IO;

namespace Sbn.Cms.Core.PropertyEditors
{
    /// <summary>
    /// Represents the configuration editorfor the markdown value editor.
    /// </summary>
    internal class MarkdownConfigurationEditor : ConfigurationEditor<MarkdownConfiguration>
    {
        public MarkdownConfigurationEditor(IIOHelper ioHelper) : base(ioHelper)
        {
        }
    }
}
