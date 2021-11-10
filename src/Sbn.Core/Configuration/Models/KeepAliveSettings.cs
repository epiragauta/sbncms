// Copyright (c) Sbn.
// See LICENSE for more details.

using System.ComponentModel;

namespace Sbn.Cms.Core.Configuration.Models
{
    /// <summary>
    /// Typed configuration options for keep alive settings.
    /// </summary>
    [SbnOptions(Constants.Configuration.ConfigKeepAlive)]
    public class KeepAliveSettings
    {
        internal const bool StaticDisableKeepAliveTask = false;

        /// <summary>
        /// Gets or sets a value indicating whether the keep alive task is disabled.
        /// </summary>
        [DefaultValue(StaticDisableKeepAliveTask)]
        public bool DisableKeepAliveTask { get; set; } = StaticDisableKeepAliveTask;

        /// <summary>
        /// Gets a value for the keep alive ping URL.
        /// </summary>
        public string KeepAlivePingUrl => "~/api/keepalive/ping";
    }
}
