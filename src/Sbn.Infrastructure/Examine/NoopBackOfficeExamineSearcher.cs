using System.Collections.Generic;
using System.Linq;
using Examine;
using Sbn.Cms.Core.Models.ContentEditing;

namespace Sbn.Cms.Infrastructure.Examine
{
    public class NoopBackOfficeExamineSearcher : IBackOfficeExamineSearcher
    {
        public IEnumerable<ISearchResult> Search(string query, SbnEntityTypes entityType, int pageSize, long pageIndex, out long totalFound,
            string searchFrom = null, bool ignoreUserStartNodes = false)
        {
            totalFound = 0;
            return Enumerable.Empty<ISearchResult>();
        }
    }
}
