// Copyright (c) Sbn.
// See LICENSE for more details.

namespace Sbn.Cms.Core.Configuration.Models
{
    /// <summary>
    /// Typed configuration options for active directory settings.
    /// </summary>
    [SbnOptions(Constants.Configuration.ConfigActiveDirectory)]
    public class ActiveDirectorySettings
    {
        /// <summary>
        /// Gets or sets a value for the Active Directory domain.
        /// </summary>
        public string Domain { get; set; }
    }
}
