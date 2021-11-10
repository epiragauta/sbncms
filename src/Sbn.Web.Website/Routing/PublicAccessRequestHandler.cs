using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Sbn.Cms.Core;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Models.PublishedContent;
using Sbn.Cms.Core.Routing;
using Sbn.Cms.Core.Security;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Web;
using Sbn.Cms.Web.Common.Routing;
using Sbn.Extensions;

namespace Sbn.Cms.Web.Website.Routing
{
    public class PublicAccessRequestHandler : IPublicAccessRequestHandler
    {
        private readonly ILogger<PublicAccessRequestHandler> _logger;
        private readonly IPublicAccessService _publicAccessService;
        private readonly IPublicAccessChecker _publicAccessChecker;
        private readonly ISbnContextAccessor _sbnContextAccessor;
        private readonly ISbnRouteValuesFactory _sbnRouteValuesFactory;
        private readonly IPublishedRouter _publishedRouter;

        public PublicAccessRequestHandler(
            ILogger<PublicAccessRequestHandler> logger,
            IPublicAccessService publicAccessService,
            IPublicAccessChecker publicAccessChecker,
            ISbnContextAccessor sbnContextAccessor,
            ISbnRouteValuesFactory sbnRouteValuesFactory,
            IPublishedRouter publishedRouter)
        {
            _logger = logger;
            _publicAccessService = publicAccessService;
            _publicAccessChecker = publicAccessChecker;
            _sbnContextAccessor = sbnContextAccessor;
            _sbnRouteValuesFactory = sbnRouteValuesFactory;
            _publishedRouter = publishedRouter;
        }

        /// <inheritdoc />
        public async Task<SbnRouteValues> RewriteForPublishedContentAccessAsync(HttpContext httpContext, SbnRouteValues routeValues)
        {
            // because these might loop, we have to have some sort of infinite loop detection
            int i = 0;
            const int maxLoop = 8;
            PublicAccessStatus publicAccessStatus = PublicAccessStatus.AccessAccepted;
            do
            {
                _logger.LogDebug(nameof(RewriteForPublishedContentAccessAsync) + ": Loop {LoopCounter}", i);


                IPublishedContent publishedContent = routeValues.PublishedRequest?.PublishedContent;
                if (publishedContent == null)
                {
                    return routeValues;
                }

                var path = publishedContent.Path;

                Attempt<PublicAccessEntry> publicAccessAttempt = _publicAccessService.IsProtected(path);

                if (publicAccessAttempt)
                {
                    _logger.LogDebug("EnsurePublishedContentAccess: Page is protected, check for access");

                    // manually authenticate the request
                    AuthenticateResult authResult = await httpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme);
                    if (authResult.Succeeded)
                    {
                        // set the user to the auth result. we need to do this here because this occurs
                        // before the authentication middleware.
                        // NOTE: It would be possible to just pass the authResult to the HasMemberAccessToContentAsync method
                        // instead of relying directly on the user assigned to the http context, and then the auth middleware
                        // will run anyways and assign the user. Perhaps that is a little cleaner, but would require more code
                        // changes right now, and really it's not any different in the end result.
                        httpContext.SetPrincipalForRequest(authResult.Principal);
                    }

                    publicAccessStatus = await _publicAccessChecker.HasMemberAccessToContentAsync(publishedContent.Id);
                    switch (publicAccessStatus)
                    {
                        case PublicAccessStatus.NotLoggedIn:
                            _logger.LogDebug("EnsurePublishedContentAccess: Not logged in, redirect to login page");
                            routeValues = await SetPublishedContentAsOtherPageAsync(httpContext, routeValues.PublishedRequest, publicAccessAttempt.Result.LoginNodeId);
                            break;
                        case PublicAccessStatus.AccessDenied:
                            _logger.LogDebug("EnsurePublishedContentAccess: Current member has not access, redirect to error page");
                            routeValues = await SetPublishedContentAsOtherPageAsync(httpContext, routeValues.PublishedRequest, publicAccessAttempt.Result.NoAccessNodeId);
                            break;
                        case PublicAccessStatus.LockedOut:
                            _logger.LogDebug("Current member is locked out, redirect to error page");
                            routeValues = await SetPublishedContentAsOtherPageAsync(httpContext, routeValues.PublishedRequest, publicAccessAttempt.Result.NoAccessNodeId);
                            break;
                        case PublicAccessStatus.NotApproved:
                            _logger.LogDebug("Current member is unapproved, redirect to error page");
                            routeValues = await SetPublishedContentAsOtherPageAsync(httpContext, routeValues.PublishedRequest, publicAccessAttempt.Result.NoAccessNodeId);
                            break;
                        case PublicAccessStatus.AccessAccepted:
                            _logger.LogDebug("Current member has access");
                            break;
                    }
                }
                else
                {
                    publicAccessStatus = PublicAccessStatus.AccessAccepted;
                    _logger.LogDebug("EnsurePublishedContentAccess: Page is not protected");
                }


                //loop until we have access or reached max loops
            } while (routeValues != null && publicAccessStatus != PublicAccessStatus.AccessAccepted && i++ < maxLoop);

            if (i == maxLoop)
            {
                _logger.LogDebug(nameof(RewriteForPublishedContentAccessAsync) + ": Looks like we are running into an infinite loop, abort");
            }

            return routeValues;
        }



        private async Task<SbnRouteValues> SetPublishedContentAsOtherPageAsync(HttpContext httpContext, IPublishedRequest publishedRequest, int pageId)
        {
            if (pageId != publishedRequest.PublishedContent.Id)
            {
                var sbnContext = _sbnContextAccessor.GetRequiredSbnContext();
                IPublishedContent publishedContent = sbnContext.PublishedSnapshot.Content.GetById(pageId);
                if (publishedContent == null)
                {
                    throw new InvalidOperationException("No content found by id " + pageId);
                }

                IPublishedRequest reRouted = await _publishedRouter.UpdateRequestAsync(publishedRequest, publishedContent);

                // we need to change the content item that is getting rendered so we have to re-create SbnRouteValues.
                SbnRouteValues updatedRouteValues = await _sbnRouteValuesFactory.CreateAsync(httpContext, reRouted);

                return updatedRouteValues;
            }
            else
            {
                throw new InvalidOperationException("Public Access rule has a redirect node set to itself, nothing can be routed.");
            }
        }
    }
}
