using System;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Sbn.Cms.Core.Cache;
using Sbn.Cms.Core.Hosting;
using Sbn.Cms.Core.Web;

namespace Sbn.Extensions
{
    /// <summary>
    /// Extension methods for the cache helper
    /// </summary>
    public static class CacheHelperExtensions
    {
        /// <summary>
        /// Outputs and caches a partial view in MVC
        /// </summary>
        /// <param name="appCaches"></param>
        /// <param name="hostingEnvironment"></param>
        /// <param name="sbnContext"></param>
        /// <param name="htmlHelper"></param>
        /// <param name="partialViewName"></param>
        /// <param name="model"></param>
        /// <param name="cacheTimeout"></param>
        /// <param name="cacheKey">used to cache the partial view, this key could change if it is cached by page or by member</param>
        /// <param name="viewData"></param>
        /// <returns></returns>
        public static IHtmlContent CachedPartialView(
            this AppCaches appCaches,
            IHostingEnvironment hostingEnvironment,
            ISbnContext sbnContext,
            IHtmlHelper htmlHelper,
            string partialViewName,
            object model,
            TimeSpan cacheTimeout,
            string cacheKey,
            ViewDataDictionary viewData = null
            )
        {
            //disable cached partials in debug mode: http://issues.sbn.org/issue/U4-5940
            //disable cached partials in preview mode: https://github.com/sbn/Sbn-CMS/issues/10384
            if (hostingEnvironment.IsDebugMode || (sbnContext?.InPreviewMode == true))
            {
                // just return a normal partial view instead
                return htmlHelper.Partial(partialViewName, model, viewData);
            }

            var result = appCaches.RuntimeCache.GetCacheItem<IHtmlContent>(
                CoreCacheHelperExtensions.PartialViewCacheKey + cacheKey,
                () => new HtmlString(htmlHelper.Partial(partialViewName, model, viewData).ToHtmlString()),
                timeout: cacheTimeout);

            return result;
        }

    }
}
