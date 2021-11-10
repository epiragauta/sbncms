// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;

namespace Sbn.Cms.Core.Configuration.Models
{
    /// <summary>
    /// Typed configuration options for the plugins.
    /// </summary>
    [SbnOptions(Constants.Configuration.ConfigPlugins)]
    public class SbnPluginSettings
    {
        

        /// <summary>
        /// Gets or sets the allowed file extensions (including the period ".") that should be accessible from the browser.
        /// </summary>
        /// WB-TODO
        public ISet<string> BrowsableFileExtensions { get; set; } = new HashSet<string>(new[]
        {
            ".html", // markup
            ".css", // styles
            ".js", // scripts
            ".jpg", ".jpeg", ".gif", ".png", ".svg", // images
            ".eot", ".ttf", ".woff", // fonts
            ".xml", ".json", ".config", // configurations
            ".lic" // license
        });
    }
}
