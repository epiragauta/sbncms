// Copyright (c) Sbn.
// See LICENSE for more details.

using Sbn.Cms.Core.IO;

namespace Sbn.Cms.Core.PropertyEditors
{
    public class MultiUrlPickerConfigurationEditor : ConfigurationEditor<MultiUrlPickerConfiguration>
    {
        public MultiUrlPickerConfigurationEditor(IIOHelper ioHelper) : base(ioHelper)
        {
        }
    }
}
