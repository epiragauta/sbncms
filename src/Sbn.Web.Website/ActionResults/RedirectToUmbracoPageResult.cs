using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Sbn.Cms.Core.IO;
using Sbn.Cms.Core.Models.PublishedContent;
using Sbn.Cms.Core.Routing;
using Sbn.Cms.Core.Web;
using Sbn.Extensions;

namespace Sbn.Cms.Web.Website.ActionResults
{
    /// <summary>
    /// Redirects to an Sbn page by Id or Entity
    /// </summary>
    public class RedirectToSbnPageResult : IActionResult, IKeepTempDataResult
    {
        private IPublishedContent _publishedContent;
        private readonly QueryString _queryString;
        private readonly IPublishedUrlProvider _publishedUrlProvider;
        private readonly ISbnContextAccessor _sbnContextAccessor;
        private string _url;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedirectToSbnPageResult"/> class.
        /// </summary>
        public RedirectToSbnPageResult(Guid key, IPublishedUrlProvider publishedUrlProvider, ISbnContextAccessor sbnContextAccessor)
        {
            Key = key;
            _publishedUrlProvider = publishedUrlProvider;
            _sbnContextAccessor = sbnContextAccessor;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RedirectToSbnPageResult"/> class.
        /// </summary>
        public RedirectToSbnPageResult(Guid key, QueryString queryString, IPublishedUrlProvider publishedUrlProvider, ISbnContextAccessor sbnContextAccessor)
        {
            Key = key;
            _queryString = queryString;
            _publishedUrlProvider = publishedUrlProvider;
            _sbnContextAccessor = sbnContextAccessor;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RedirectToSbnPageResult"/> class.
        /// </summary>
        public RedirectToSbnPageResult(IPublishedContent publishedContent, IPublishedUrlProvider publishedUrlProvider, ISbnContextAccessor sbnContextAccessor)
        {
            _publishedContent = publishedContent;
            Key = publishedContent.Key;
            _publishedUrlProvider = publishedUrlProvider;
            _sbnContextAccessor = sbnContextAccessor;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RedirectToSbnPageResult"/> class.
        /// </summary>
        public RedirectToSbnPageResult(IPublishedContent publishedContent, QueryString queryString, IPublishedUrlProvider publishedUrlProvider, ISbnContextAccessor sbnContextAccessor)
        {
            _publishedContent = publishedContent;
            Key = publishedContent.Key;
            _queryString = queryString;
            _publishedUrlProvider = publishedUrlProvider;
            _sbnContextAccessor = sbnContextAccessor;
        }

        private string Url
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_url))
                {
                    return _url;
                }

                if (PublishedContent is null)
                {
                    throw new InvalidOperationException($"Cannot redirect, no entity was found for key {Key}");
                }

                var result = _publishedUrlProvider.GetUrl(PublishedContent.Id);

                if (result == "#")
                {
                    throw new InvalidOperationException(
                        $"Could not route to entity with key {Key}, the NiceUrlProvider could not generate a URL");
                }

                _url = result;

                return _url;
            }
        }

        public Guid Key { get; }

        private IPublishedContent PublishedContent
        {
            get
            {
                if (!(_publishedContent is null))
                {
                    return _publishedContent;
                }

                // need to get the URL for the page
                _publishedContent = _sbnContextAccessor.GetRequiredSbnContext().Content.GetById(Key);

                return _publishedContent;
            }
        }

        /// <inheritdoc/>
        public Task ExecuteResultAsync(ActionContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            HttpContext httpContext = context.HttpContext;
            IIOHelper ioHelper = httpContext.RequestServices.GetRequiredService<IIOHelper>();
            string destinationUrl = ioHelper.ResolveUrl(Url);

            if (_queryString.HasValue)
            {
                destinationUrl += _queryString.ToUriComponent();
            }

            httpContext.Response.Redirect(destinationUrl);

            return Task.CompletedTask;
        }

    }
}
