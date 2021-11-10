// Copyright (c) Sbn.
// See LICENSE for more details.

namespace Sbn.Cms.Core.Configuration.Models
{
    /// <summary>
    /// Typed configuration options for connection strings.
    /// </summary>
    [SbnOptions("ConnectionStrings", BindNonPublicProperties = true)]
    public class ConnectionStrings
    {
        // Backing field for SbnConnectionString to load from configuration value with key sbnDbDSN.
        // Attributes cannot be applied to map from keys that don't match, and have chosen to retain the key name
        // used in configuration for older Sbn versions.
        // See: https://stackoverflow.com/a/54607296/489433
#pragma warning disable SA1300  // Element should begin with upper-case letter
#pragma warning disable IDE1006 // Naming Styles
        private string sbnDbDSN
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore SA1300  // Element should begin with upper-case letter
        {
            get => SbnConnectionString?.ConnectionString;
            set => SbnConnectionString = new ConfigConnectionString(Constants.System.SbnConnectionName, value);
        }

        /// <summary>
        /// Gets or sets a value for the Sbn database connection string..
        /// </summary>
        public ConfigConnectionString SbnConnectionString { get; set; } = new ConfigConnectionString(Constants.System.SbnConnectionName, null);
    }
}
