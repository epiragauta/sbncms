// Copyright (c) Sbn.
// See LICENSE for more details.

using Sbn.Cms.Core.Configuration;

namespace Sbn.Extensions
{
    public static class ConfigConnectionStringExtensions
    {
        public static bool IsConnectionStringConfigured(this ConfigConnectionString databaseSettings)
            => databaseSettings != null &&
            !string.IsNullOrWhiteSpace(databaseSettings.ConnectionString) &&
            !string.IsNullOrWhiteSpace(databaseSettings.ProviderName);
    }
}
