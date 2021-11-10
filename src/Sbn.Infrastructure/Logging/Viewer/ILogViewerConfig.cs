using System;
using System.Collections.Generic;

namespace Sbn.Cms.Core.Logging.Viewer
{
    public interface ILogViewerConfig
    {
        IReadOnlyList<SavedLogSearch> GetSavedSearches();
        IReadOnlyList<SavedLogSearch> AddSavedSearch(string name, string query);
        IReadOnlyList<SavedLogSearch> DeleteSavedSearch(string name, string query);
    }
}
