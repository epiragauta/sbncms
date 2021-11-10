// Copyright (c) Sbn.
// See LICENSE for more details.

using Sbn.Cms.Core;
using Sbn.Cms.Core.Web;

namespace Sbn.Extensions
{
    public static class CookieManagerExtensions
    {
        public static string GetPreviewCookieValue(this ICookieManager cookieManager)
        {
            return cookieManager.GetCookieValue(Constants.Web.PreviewCookieName);
        }

    }

}
