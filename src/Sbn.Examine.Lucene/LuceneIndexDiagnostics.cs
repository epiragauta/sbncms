// Copyright (c) Sbn.
// See LICENSE for more details.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Examine.Lucene;
using Examine.Lucene.Providers;
using Lucene.Net.Store;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sbn.Cms.Core;
using Sbn.Cms.Core.Hosting;
using Sbn.Extensions;

namespace Sbn.Cms.Infrastructure.Examine
{
    public class LuceneIndexDiagnostics : IIndexDiagnostics
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly LuceneDirectoryIndexOptions _indexOptions;

        public LuceneIndexDiagnostics(
            LuceneIndex index,
            ILogger<LuceneIndexDiagnostics> logger,
            IHostingEnvironment hostingEnvironment,
            IOptionsMonitor<LuceneDirectoryIndexOptions> indexOptions)
        {
            _hostingEnvironment = hostingEnvironment;
            _indexOptions = indexOptions.Get(index.Name);
            Index = index;
            Logger = logger;
        }

        public LuceneIndex Index { get; }
        public ILogger<LuceneIndexDiagnostics> Logger { get; }

       

        public Attempt<string> IsHealthy()
        {
            var isHealthy = Index.IsHealthy(out var indexError);
            return isHealthy ? Attempt<string>.Succeed() : Attempt.Fail(indexError.Message);
        }

        public long GetDocumentCount() => Index.GetDocumentCount();

        public IEnumerable<string> GetFieldNames() => Index.GetFieldNames();

        public virtual IReadOnlyDictionary<string, object> Metadata
        {
            get
            {
                Directory luceneDir = Index.GetLuceneDirectory();
                var d = new Dictionary<string, object>
                {
                    [nameof(SbnExamineIndex.CommitCount)] = Index.CommitCount,
                    [nameof(SbnExamineIndex.DefaultAnalyzer)] = Index.DefaultAnalyzer.GetType().Name,
                    ["LuceneDirectory"] = luceneDir.GetType().Name
                };

                if (luceneDir is FSDirectory fsDir)
                {

                    var rootDir = _hostingEnvironment.ApplicationPhysicalPath;
                    d["LuceneIndexFolder"] = fsDir.Directory.ToString().ToLowerInvariant().TrimStart(rootDir.ToLowerInvariant()).Replace("\\", " /").EnsureStartsWith('/');
                }

                if (_indexOptions != null)
                {
                    if (_indexOptions.DirectoryFactory != null)
                    {
                        d[nameof(LuceneDirectoryIndexOptions.DirectoryFactory)] = _indexOptions.DirectoryFactory.GetType();
                    }
                    
                    if (_indexOptions.IndexDeletionPolicy != null)
                    {
                        d[nameof(LuceneDirectoryIndexOptions.IndexDeletionPolicy)] = _indexOptions.IndexDeletionPolicy.GetType();
                    } 
                    
                }

                return d;
            }
        }


    }
}
