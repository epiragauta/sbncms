// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.IO;

namespace Sbn.Cms.Core.PropertyEditors
{
    /// <summary>
    /// Represents the configuration editor for the image cropper value editor.
    /// </summary>
    internal class ImageCropperConfigurationEditor : ConfigurationEditor<ImageCropperConfiguration>
    {
        /// <inheritdoc />
        public override IDictionary<string, object> ToValueEditor(object configuration)
        {
            var d = base.ToValueEditor(configuration);
            if (!d.ContainsKey("focalPoint")) d["focalPoint"] = new { left = 0.5, top = 0.5 };
            if (!d.ContainsKey("src")) d["src"] = "";
            return d;
        }

        public ImageCropperConfigurationEditor(IIOHelper ioHelper) : base(ioHelper)
        {
        }
    }
}
