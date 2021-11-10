// Copyright (c) Sbn.
// See LICENSE for more details.

using Sbn.Cms.Core.IO;

namespace Sbn.Cms.Core.PropertyEditors
{
    /// <summary>
    /// Represents the configuration editor for the nested content value editor.
    /// </summary>
    public class NestedContentConfigurationEditor : ConfigurationEditor<NestedContentConfiguration>
    {
        public NestedContentConfigurationEditor(IIOHelper ioHelper) : base(ioHelper)
        {
        }
    }
}
