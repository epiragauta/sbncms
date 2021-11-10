using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sbn.Cms.Core.Cache;
using Sbn.Cms.Core.Logging;
using Sbn.Cms.Core.Models.PublishedContent;
using Sbn.Cms.Core.Routing;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Web;
using Sbn.Cms.Infrastructure.Persistence;
using Sbn.Cms.Web.Common.Controllers;
using Sbn.Cms.Web.Common.Routing;
using Sbn.Cms.Web.Website.ActionResults;

namespace Sbn.Cms.Web.Website.Controllers
{
    /// <summary>
    /// Provides a base class for front-end add-in controllers.
    /// </summary>
    [AutoValidateAntiforgeryToken]
    public abstract class SurfaceController : PluginController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SurfaceController"/> class.
        /// </summary>
        protected SurfaceController(ISbnContextAccessor sbnContextAccessor, ISbnDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider)
            : base(sbnContextAccessor, databaseFactory, services, appCaches, profilingLogger)
            => PublishedUrlProvider = publishedUrlProvider;

        protected IPublishedUrlProvider PublishedUrlProvider { get; }

        /// <summary>
        /// Gets the current page.
        /// </summary>
        protected virtual IPublishedContent CurrentPage
        {
            get
            {
                SbnRouteValues sbnRouteValues = HttpContext.Features.Get<SbnRouteValues>();
                if (sbnRouteValues is null)
                {
                    throw new InvalidOperationException($"No {nameof(SbnRouteValues)} feature was found in the HttpContext");
                }

                return sbnRouteValues.PublishedRequest.PublishedContent;
            }
        }

        /// <summary>
        /// Redirects to the Sbn page with the given id
        /// </summary>
        protected RedirectToSbnPageResult RedirectToSbnPage(Guid contentKey)
            => new RedirectToSbnPageResult(contentKey, PublishedUrlProvider, SbnContextAccessor);

        /// <summary>
        /// Redirects to the Sbn page with the given id and passes provided querystring
        /// </summary>
        protected RedirectToSbnPageResult RedirectToSbnPage(Guid contentKey, QueryString queryString)
            => new RedirectToSbnPageResult(contentKey, queryString, PublishedUrlProvider, SbnContextAccessor);

        /// <summary>
        /// Redirects to the Sbn page with the given published content
        /// </summary>
        protected RedirectToSbnPageResult RedirectToSbnPage(IPublishedContent publishedContent)
            => new RedirectToSbnPageResult(publishedContent, PublishedUrlProvider, SbnContextAccessor);

        /// <summary>
        /// Redirects to the Sbn page with the given published content and passes provided querystring
        /// </summary>
        protected RedirectToSbnPageResult RedirectToSbnPage(IPublishedContent publishedContent, QueryString queryString)
            => new RedirectToSbnPageResult(publishedContent, queryString, PublishedUrlProvider, SbnContextAccessor);

        /// <summary>
        /// Redirects to the currently rendered Sbn page
        /// </summary>
        protected RedirectToSbnPageResult RedirectToCurrentSbnPage()
            => new RedirectToSbnPageResult(CurrentPage, PublishedUrlProvider, SbnContextAccessor);

        /// <summary>
        /// Redirects to the currently rendered Sbn page and passes provided querystring
        /// </summary>
        protected RedirectToSbnPageResult RedirectToCurrentSbnPage(QueryString queryString)
            => new RedirectToSbnPageResult(CurrentPage, queryString, PublishedUrlProvider, SbnContextAccessor);

        /// <summary>
        /// Redirects to the currently rendered Sbn URL
        /// </summary>
        /// <remarks>
        /// This is useful if you need to redirect
        /// to the current page but the current page is actually a rewritten URL normally done with something like
        /// Server.Transfer.*
        /// </remarks>
        protected RedirectToSbnUrlResult RedirectToCurrentSbnUrl()
            => new RedirectToSbnUrlResult(SbnContext);

        /// <summary>
        /// Returns the currently rendered Sbn page
        /// </summary>
        protected SbnPageResult CurrentSbnPage()
        {
            HttpContext.Features.Set(new ProxyViewDataFeature(ViewData, TempData));
            return new SbnPageResult(ProfilingLogger);
        }
    }
}
