using System;
using System.Globalization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sbn.Cms.Core;
using Sbn.Cms.Core.PropertyEditors.ValueConverters;

namespace Sbn.Extensions
{
    /// <summary>
    /// Provides extension methods for getting ImageProcessor URL from the core Image Cropper property editor
    /// </summary>
    public static class ImageCropperTemplateExtensions
    {

        private static readonly JsonSerializerSettings s_imageCropperValueJsonSerializerSettings = new JsonSerializerSettings
        {
            Culture = CultureInfo.InvariantCulture,
            FloatParseHandling = FloatParseHandling.Decimal
        };

        internal static ImageCropperValue DeserializeImageCropperValue(this string json)
        {
            ImageCropperValue imageCrops = null;

            if (json.DetectIsJson())
            {
                try
                {
                    imageCrops = JsonConvert.DeserializeObject<ImageCropperValue>(json, s_imageCropperValueJsonSerializerSettings);
                }
                catch (Exception ex)
                {
                    StaticApplicationLogging.Logger.LogError(ex, "Could not parse the json string: {Json}", json);
                }
            }

            imageCrops ??= new ImageCropperValue();
            return imageCrops;

        }
    }
}
