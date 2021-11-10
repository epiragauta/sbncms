// Copyright (c) Sbn.
// See LICENSE for more details.

using Sbn.Cms.Core.Web;

namespace Sbn.Extensions
{
    public static class SbnContextExtensions
    {
        /// <summary>
        /// Boolean value indicating whether the current request is a front-end sbn request
        /// </summary>
        public static bool IsFrontEndSbnRequest(this ISbnContext sbnContext) => sbnContext.PublishedRequest != null;
    }
}
