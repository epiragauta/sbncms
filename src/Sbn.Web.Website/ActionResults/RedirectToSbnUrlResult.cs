using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Sbn.Cms.Core.Web;

namespace Sbn.Cms.Web.Website.ActionResults
{
    /// <summary>
    /// Redirects to the current URL rendering an Sbn page including it's query strings
    /// </summary>
    /// <remarks>
    /// This is useful if you need to redirect
    /// to the current page but the current page is actually a rewritten URL normally done with something like
    /// Server.Transfer. It is also handy if you want to persist the query strings.
    /// </remarks>
    public class RedirectToSbnUrlResult : IActionResult, IKeepTempDataResult
    {
        private readonly ISbnContext _sbnContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedirectToSbnUrlResult"/> class.
        /// </summary>
        public RedirectToSbnUrlResult(ISbnContext sbnContext) => _sbnContext = sbnContext;

        /// <inheritdoc/>
        public Task ExecuteResultAsync(ActionContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var destinationUrl = _sbnContext.OriginalRequestUrl.PathAndQuery;

            context.HttpContext.Response.Redirect(destinationUrl);

            return Task.CompletedTask;
        }
    }
}
