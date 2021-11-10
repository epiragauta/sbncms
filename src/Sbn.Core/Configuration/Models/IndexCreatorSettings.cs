// Copyright (c) Sbn.
// See LICENSE for more details.

using System;

namespace Sbn.Cms.Core.Configuration.Models
{
    /// <summary>
    /// Typed configuration options for index creator settings.
    /// </summary>
    [SbnOptions(Constants.Configuration.ConfigExamine)]
    public class IndexCreatorSettings
    {
        /// <summary>
        /// Gets or sets a value for lucene directory factory type.
        /// </summary>
        public LuceneDirectoryFactory LuceneDirectoryFactory { get; set; }

    }
}
