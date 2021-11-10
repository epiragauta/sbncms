// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Globalization;
using Microsoft.AspNetCore.Mvc.Filters;
using Sbn.Extensions;

namespace Sbn.Cms.Web.Common.Filters
{
    /// <summary>
    /// Applied to all Sbn controllers to ensure the thread culture is set to the culture assigned to the back office identity
    /// </summary>
    public class BackOfficeCultureFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var culture = context.HttpContext.User.Identity.GetCulture();
            if (culture != null)
            {
                SetCurrentThreadCulture(culture);
            }
        }

        private static void SetCurrentThreadCulture(CultureInfo culture)
        {
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
        }
    }


}
