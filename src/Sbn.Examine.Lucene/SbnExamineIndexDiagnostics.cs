// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using System.Linq;
using Examine.Lucene;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sbn.Cms.Core.Hosting;

namespace Sbn.Cms.Infrastructure.Examine
{
    public class SbnExamineIndexDiagnostics : LuceneIndexDiagnostics
    {
        private readonly SbnExamineIndex _index;

        public SbnExamineIndexDiagnostics(
            SbnExamineIndex index,
            ILogger<SbnExamineIndexDiagnostics> logger,
            IHostingEnvironment hostingEnvironment,
            IOptionsMonitor<LuceneDirectoryIndexOptions> indexOptions)
            : base(index, logger, hostingEnvironment, indexOptions)
        {
            _index = index;
        }

        public override IReadOnlyDictionary<string, object> Metadata
        {
            get
            {
                var d = base.Metadata.ToDictionary(x => x.Key, x => x.Value);

                d[nameof(SbnExamineIndex.EnableDefaultEventHandler)] = _index.EnableDefaultEventHandler;
                d[nameof(SbnExamineIndex.PublishedValuesOnly)] = _index.PublishedValuesOnly;

                if (_index.ValueSetValidator is ValueSetValidator vsv)
                {
                    d[nameof(ValueSetValidator.IncludeItemTypes)] = vsv.IncludeItemTypes;
                    d[nameof(ContentValueSetValidator.ExcludeItemTypes)] = vsv.ExcludeItemTypes;
                    d[nameof(ContentValueSetValidator.IncludeFields)] = vsv.IncludeFields;
                    d[nameof(ContentValueSetValidator.ExcludeFields)] = vsv.ExcludeFields;
                }

                if (_index.ValueSetValidator is ContentValueSetValidator cvsv)
                {
                    d[nameof(ContentValueSetValidator.PublishedValuesOnly)] = cvsv.PublishedValuesOnly;
                    d[nameof(ContentValueSetValidator.SupportProtectedContent)] = cvsv.SupportProtectedContent;
                    d[nameof(ContentValueSetValidator.ParentId)] = cvsv.ParentId;
                }

                return d.Where(x => x.Value != null).ToDictionary(x => x.Key, x => x.Value);
            }
        }
    }
}
