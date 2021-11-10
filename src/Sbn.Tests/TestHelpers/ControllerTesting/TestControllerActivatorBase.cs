using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using Moq;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.Models.Membership;
using Sbn.Cms.Core.Models.PublishedContent;
using Sbn.Cms.Core.PublishedCache;
using Sbn.Cms.Core.Routing;
using Sbn.Cms.Core.Security;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Web;
using Sbn.Cms.Tests.Common;
using Sbn.Extensions;
using Sbn.Tests.TestHelpers.Entities;
using Sbn.Web;
using Sbn.Web.WebApi;

namespace Sbn.Tests.TestHelpers.ControllerTesting
{
    /// <summary>
    /// Used to mock all of the services required for re-mocking and testing controllers
    /// </summary>
    /// <remarks>
    /// A more complete version of this is found in the Sbn REST API project but this has the basics covered
    /// </remarks>
    public abstract class TestControllerActivatorBase : DefaultHttpControllerActivator, IHttpControllerActivator
    {
        IHttpController IHttpControllerActivator.Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            // default
            if (!typeof (SbnApiControllerBase).IsAssignableFrom(controllerType))
                return base.Create(request, controllerDescriptor, controllerType);

            var owinContext = request.TryGetOwinContext().Result;

            var mockedUserService = Mock.Of<IUserService>();
            var mockedContentService = Mock.Of<IContentService>();
            var mockedMediaService = Mock.Of<IMediaService>();
            var mockedEntityService = Mock.Of<IEntityService>();
            var mockedMemberService = Mock.Of<IMemberService>();
            var mockedMemberTypeService = Mock.Of<IMemberTypeService>();
            var mockedDataTypeService = Mock.Of<IDataTypeService>();
            var mockedContentTypeService = Mock.Of<IContentTypeService>();

            var serviceContext = ServiceContext.CreatePartial(
                userService: mockedUserService,
                contentService: mockedContentService,
                mediaService: mockedMediaService,
                entityService: mockedEntityService,
                memberService: mockedMemberService,
                memberTypeService: mockedMemberTypeService,
                dataTypeService: mockedDataTypeService,
                contentTypeService: mockedContentTypeService,
                localizedTextService:Mock.Of<ILocalizedTextService>());

            var globalSettings = new GlobalSettings();

            // FIXME: v8?
            ////new app context
            //var dbCtx = new Mock<DatabaseContext>(Mock.Of<IDatabaseFactory>(), Mock.Of<ILogger>(), Mock.Of<ISqlSyntaxProvider>(), "test");
            ////ensure these are set so that the appctx is 'Configured'
            //dbCtx.Setup(x => x.CanConnect).Returns(true);
            //dbCtx.Setup(x => x.IsDatabaseConfigured).Returns(true);
            //var appCtx = ApplicationContext.EnsureContext(
            //    dbCtx.Object,
            //    //pass in mocked services
            //    serviceContext,
            //    CacheHelper.CreateDisabledCacheHelper(),
            //    new ProfilingLogger(Mock.Of<ILogger>(), Mock.Of<IProfiler>()),
            //    true);

            var httpContextItems = new Dictionary<string, object>
            {
                //add the special owin environment to the httpcontext items, this is how the GetOwinContext works
                ["owin.Environment"] = new Dictionary<string, object>()
            };

            //httpcontext with an auth'd user
            var httpContext = Mock.Of<HttpContextBase>(
                http => http.User == owinContext.Authentication.User
                        //ensure the request exists with a cookies collection
                        && http.Request == Mock.Of<HttpRequestBase>(r => r.Cookies == new HttpCookieCollection()
                            && r.RequestContext == new System.Web.Routing.RequestContext
                            {
                                RouteData = new System.Web.Routing.RouteData()
                            })
                        //ensure the request exists with an items collection
                        && http.Items == httpContextItems);
            //chuck it into the props since this is what MS does when hosted and it's needed there
            request.Properties["MS_HttpContext"] = httpContext;

            var backofficeIdentity = (ClaimsIdentity) owinContext.Authentication.User.Identity;

            var backofficeSecurity = new Mock<IBackOfficeSecurity>();

            //mock CurrentUser
            var groups = new List<ReadOnlyUserGroup>();
            for (var index = 0; index < backofficeIdentity.GetRoles().Length; index++)
            {
                var role = backofficeIdentity.GetRoles()[index];
                groups.Add(new ReadOnlyUserGroup(index + 1, role, "icon-user", null, null, role, new string[0], new string[0]));
            }
            var mockUser = MockedUser.GetUserMock();
            mockUser.Setup(x => x.IsApproved).Returns(true);
            mockUser.Setup(x => x.IsLockedOut).Returns(false);
            mockUser.Setup(x => x.AllowedSections).Returns(backofficeIdentity.GetAllowedApplications());
            mockUser.Setup(x => x.Groups).Returns(groups);
            mockUser.Setup(x => x.Email).Returns("admin@admin.com");
            mockUser.Setup(x => x.Id).Returns((int)backofficeIdentity.GetId());
            mockUser.Setup(x => x.Language).Returns("en");
            mockUser.Setup(x => x.Name).Returns(backofficeIdentity.GetRealName());
            mockUser.Setup(x => x.StartContentIds).Returns(backofficeIdentity.GetStartContentNodes());
            mockUser.Setup(x => x.StartMediaIds).Returns(backofficeIdentity.GetStartMediaNodes());
            mockUser.Setup(x => x.Username).Returns(backofficeIdentity.GetUsername());
            backofficeSecurity.Setup(x => x.CurrentUser)
                .Returns(mockUser.Object);

            //mock Validate
            backofficeSecurity.Setup(x => x.UserHasSectionAccess(It.IsAny<string>(), It.IsAny<IUser>()))
                .Returns(() => true);

            var publishedSnapshot = new Mock<IPublishedSnapshot>();
            publishedSnapshot.Setup(x => x.Members).Returns(Mock.Of<IPublishedMemberCache>());
            var publishedSnapshotService = new Mock<IPublishedSnapshotService>();
            publishedSnapshotService.Setup(x => x.CreatePublishedSnapshot(It.IsAny<string>())).Returns(publishedSnapshot.Object);

            var sbnContextAccessor = Sbn.Web.Composing.Current.SbnContextAccessor;

            var httpContextAccessor = TestHelper.GetHttpContextAccessor(httpContext);
            var umbCtx = new SbnContext(httpContextAccessor,
                publishedSnapshotService.Object,
                backofficeSecurity.Object,
                globalSettings,
                TestHelper.GetHostingEnvironment(),
                new TestVariationContextAccessor(),
                TestHelper.UriUtility,
                new AspNetCookieManager(httpContextAccessor));

            //replace it
            sbnContextAccessor.SbnContext = umbCtx;

            var urlHelper = new Mock<IUrlProvider>();
            urlHelper.Setup(provider => provider.GetUrl(It.IsAny<IPublishedContent>(), It.IsAny<UrlMode>(), It.IsAny<string>(), It.IsAny<Uri>()))
                .Returns(UrlInfo.Url("/hello/world/1234"));

            return CreateController(controllerType, request, sbnContextAccessor);
        }

        protected abstract ApiController CreateController(Type controllerType, HttpRequestMessage msg, ISbnContextAccessor sbnContextAccessor);
    }
}
