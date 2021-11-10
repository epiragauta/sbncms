// Copyright (c) Sbn.
// See LICENSE for more details.

using Sbn.Cms.Core.Cache;

namespace Sbn.Extensions
{
    /// <summary>
    /// Extension methods for the cache helper
    /// </summary>
    public static class CoreCacheHelperExtensions
    {
        public const string PartialViewCacheKey = "Sbn.Web.PartialViewCacheKey";

        /// <summary>
        /// Clears the cache for partial views
        /// </summary>
        /// <param name="appCaches"></param>
        public static void ClearPartialViewCache(this AppCaches appCaches)
        {
            appCaches.RuntimeCache.ClearByKey(PartialViewCacheKey);
        }
    }
}
