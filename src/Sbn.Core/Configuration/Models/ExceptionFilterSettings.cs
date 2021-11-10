// Copyright (c) Sbn.
// See LICENSE for more details.

using System.ComponentModel;

namespace Sbn.Cms.Core.Configuration.Models
{
    /// <summary>
    /// Typed configuration options for exception filter settings.
    /// </summary>
    [SbnOptions(Constants.Configuration.ConfigExceptionFilter)]
    public class ExceptionFilterSettings
    {
        internal const bool StaticDisabled = false;

        /// <summary>
        /// Gets or sets a value indicating whether the exception filter is disabled.
        /// </summary>
        [DefaultValue(StaticDisabled)]
        public bool Disabled { get; set; } = StaticDisabled;
    }
}
