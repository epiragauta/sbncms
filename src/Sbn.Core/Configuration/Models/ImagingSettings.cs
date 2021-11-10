// Copyright (c) Sbn.
// See LICENSE for more details.

namespace Sbn.Cms.Core.Configuration.Models
{
    /// <summary>
    /// Typed configuration options for imaging settings.
    /// </summary>
    [SbnOptions(Constants.Configuration.ConfigImaging)]
    public class ImagingSettings
    {
        /// <summary>
        /// Gets or sets a value for imaging cache settings.
        /// </summary>
        public ImagingCacheSettings Cache { get; set; } = new ImagingCacheSettings();

        /// <summary>
        /// Gets or sets a value for imaging resize settings.
        /// </summary>
        public ImagingResizeSettings Resize { get; set; } = new ImagingResizeSettings();
    }
}
