// Copyright (c) Sbn.
// See LICENSE for more details.

namespace Sbn.Cms.Core.Configuration.Models
{
    /// <summary>
    /// Typed configuration options for runtime settings.
    /// </summary>
    [SbnOptions(Constants.Configuration.ConfigRuntime)]
    public class RuntimeSettings
    {
        /// <summary>
        /// Gets or sets a value for the maximum query string length.
        /// </summary>
        public int? MaxQueryStringLength { get; set; }

        /// <summary>
        /// Gets or sets a value for the maximum request length in kb.
        /// </summary>
        public int? MaxRequestLength { get; set; }
    }
}
