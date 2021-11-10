// Copyright (c) Sbn.
// See LICENSE for more details.

using Sbn.Cms.Core.IO;

namespace Sbn.Cms.Core.PropertyEditors
{
    /// <summary>
    /// Represents the configuration editor for the slider value editor.
    /// </summary>
    public class SliderConfigurationEditor : ConfigurationEditor<SliderConfiguration>
    {
        public SliderConfigurationEditor(IIOHelper ioHelper) : base(ioHelper)
        {
        }
    }
}
