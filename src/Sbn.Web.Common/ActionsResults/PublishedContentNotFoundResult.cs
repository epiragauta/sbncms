using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sbn.Cms.Core.Routing;
using Sbn.Cms.Core.Web;

namespace Sbn.Cms.Web.Common.ActionsResults
{
    /// <summary>
    /// Returns the Sbn not found result
    /// </summary>
    public class PublishedContentNotFoundResult : IActionResult
    {
        private readonly ISbnContext _sbnContext;
        private readonly string _message;

        /// <summary>
        /// Initializes a new instance of the <see cref="PublishedContentNotFoundResult"/> class.
        /// </summary>
        public PublishedContentNotFoundResult(ISbnContext sbnContext, string message = null)
        {
            _sbnContext = sbnContext;
            _message = message;
        }

        /// <inheritdoc/>
        public async Task ExecuteResultAsync(ActionContext context)
        {
            HttpResponse response = context.HttpContext.Response;

            response.Clear();

            response.StatusCode = StatusCodes.Status404NotFound;

            IPublishedRequest frequest = _sbnContext.PublishedRequest;
            var reason = "Cannot render the page at URL '{0}'.";
            if (frequest.HasPublishedContent() == false)
            {
                reason = "No sbn document matches the URL '{0}'.";
            }
            else if (frequest.HasTemplate() == false)
            {
                reason = "No template exists to render the document at URL '{0}'.";
            }

            await response.WriteAsync("<html><body><h1>Page not found</h1>");
            await response.WriteAsync("<h2>");
            await response.WriteAsync(string.Format(reason, WebUtility.HtmlEncode(_sbnContext.OriginalRequestUrl.PathAndQuery)));
            await response.WriteAsync("</h2>");
            if (string.IsNullOrWhiteSpace(_message) == false)
            {
                await response.WriteAsync("<p>" + _message + "</p>");
            }

            await response.WriteAsync("<p>This page can be replaced with a custom 404. Check the <a target='_blank' href='https://umbra.co/custom-error-pages'>documentation for <a href=\"https://our.sbn.com/Documentation/Tutorials/Custom-Error-Pages/#404-errors\" target=\"_blank\">Custom 404 Error Pages</a>.</p>");
            await response.WriteAsync("<p style=\"border-top: 1px solid #ccc; padding-top: 10px\"><small>This page is intentionally left ugly ;-)</small></p>");
            await response.WriteAsync("</body></html>");
        }
    }
}
