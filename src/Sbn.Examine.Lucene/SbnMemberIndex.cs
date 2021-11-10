// Copyright (c) Sbn.
// See LICENSE for more details.

using Examine.Lucene;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sbn.Cms.Core.Hosting;
using Sbn.Cms.Core.Services;

namespace Sbn.Cms.Infrastructure.Examine
{
    /// <summary>
    /// Custom indexer for members
    /// </summary>
    public class SbnMemberIndex : SbnExamineIndex, ISbnMemberIndex
    {
        public SbnMemberIndex(
            ILoggerFactory loggerFactory,
            string name,
            IOptionsMonitor<LuceneDirectoryIndexOptions> indexOptions,
            IHostingEnvironment hostingEnvironment,
            IRuntimeState runtimeState)
            : base(loggerFactory, name, indexOptions, hostingEnvironment, runtimeState)
        {
        }
    }
}
