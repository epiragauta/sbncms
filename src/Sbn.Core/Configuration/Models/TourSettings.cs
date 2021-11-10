// Copyright (c) Sbn.
// See LICENSE for more details.

using System.ComponentModel;

namespace Sbn.Cms.Core.Configuration.Models
{
    /// <summary>
    /// Typed configuration options for tour settings.
    /// </summary>
    [SbnOptions(Constants.Configuration.ConfigTours)]
    public class TourSettings
    {
        internal const bool StaticEnableTours = true;

        /// <summary>
        /// Gets or sets a value indicating whether back-office tours are enabled.
        /// </summary>
        [DefaultValue(StaticEnableTours)]
        public bool EnableTours { get; set; } = StaticEnableTours;
    }
}
