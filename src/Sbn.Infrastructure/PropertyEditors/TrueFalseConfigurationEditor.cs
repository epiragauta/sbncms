// Copyright (c) Sbn.
// See LICENSE for more details.

using Sbn.Cms.Core.IO;

namespace Sbn.Cms.Core.PropertyEditors
{
    /// <summary>
    /// Represents the configuration editor for the boolean value editor.
    /// </summary>
    public class TrueFalseConfigurationEditor : ConfigurationEditor<TrueFalseConfiguration>
    {
        public TrueFalseConfigurationEditor(IIOHelper ioHelper) : base(ioHelper)
        {
        }
    }
}
