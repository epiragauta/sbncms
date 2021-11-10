using System.Collections.Generic;
using Examine;
using Sbn.Cms.Core.Models.ContentEditing;

namespace Sbn.Cms.Infrastructure.Examine
{
    /// <summary>
    /// Used to search the back office for Examine indexed entities (Documents, Media and Members)
    /// </summary>
    public interface IBackOfficeExamineSearcher
    {
        IEnumerable<ISearchResult> Search(string query,
            SbnEntityTypes entityType,
            int pageSize,
            long pageIndex, out long totalFound, string searchFrom = null, bool ignoreUserStartNodes = false);
    }
}
