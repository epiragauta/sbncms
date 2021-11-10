// Copyright (c) Sbn.
// See LICENSE for more details.

using Sbn.Cms.Core.IO;

namespace Sbn.Cms.Core.PropertyEditors
{
    /// <summary>
    /// Represents the configuration editor for the listview value editor.
    /// </summary>
    public class ListViewConfigurationEditor : ConfigurationEditor<ListViewConfiguration>
    {
        public ListViewConfigurationEditor(IIOHelper ioHelper) : base(ioHelper)
        {
        }
    }
}
