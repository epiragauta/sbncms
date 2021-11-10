// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Moq;
using NUnit.Framework;
using Sbn.Cms.Core.Cache;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Models.Membership;
using Sbn.Cms.Core.Security;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Tests.Common.Builders;
using Sbn.Cms.Web.BackOffice.Authorization;
using Sbn.Extensions;
using Constants = Sbn.Cms.Core.Constants;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Web.BackOffice.Authorization
{
    public class ContentPermissionsResourceHandlerTests
    {
        private const int NodeId = 1000;

        [Test]
        public async Task Resource_With_Node_Id_With_Permission_Is_Authorized()
        {
            AuthorizationHandlerContext authHandlerContext = CreateAuthorizationHandlerContext(NodeId, createWithNodeId: true);
            ContentPermissionsResourceHandler sut = CreateHandler(NodeId, new string[] { "A" });

            await sut.HandleAsync(authHandlerContext);

            Assert.IsTrue(authHandlerContext.HasSucceeded);
        }

        [Test]
        public async Task Resource_With_Content_With_Permission_Is_Authorized()
        {
            AuthorizationHandlerContext authHandlerContext = CreateAuthorizationHandlerContext(NodeId);
            ContentPermissionsResourceHandler sut = CreateHandler(NodeId, new string[] { "A" });

            await sut.HandleAsync(authHandlerContext);

            Assert.IsTrue(authHandlerContext.HasSucceeded);
        }

        [Test]
        public async Task Resource_With_Node_Id_Withou_Permission_Is_Not_Authorized()
        {
            AuthorizationHandlerContext authHandlerContext = CreateAuthorizationHandlerContext(NodeId, createWithNodeId: true);
            ContentPermissionsResourceHandler sut = CreateHandler(NodeId, new string[] { "B" });

            await sut.HandleAsync(authHandlerContext);

            Assert.IsFalse(authHandlerContext.HasSucceeded);
        }

        [Test]
        public async Task Resource_With_Content_Without_Permission_Is_Not_Authorized()
        {
            AuthorizationHandlerContext authHandlerContext = CreateAuthorizationHandlerContext(NodeId);
            ContentPermissionsResourceHandler sut = CreateHandler(NodeId, new string[] { "B" });

            await sut.HandleAsync(authHandlerContext);

            Assert.IsFalse(authHandlerContext.HasSucceeded);
        }

        private static AuthorizationHandlerContext CreateAuthorizationHandlerContext(int nodeId, bool createWithNodeId = false)
        {
            var requirement = new ContentPermissionsResourceRequirement();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>()));
            IContent content = CreateContent(nodeId);
            System.Collections.ObjectModel.ReadOnlyCollection<char> permissions = new List<char> { 'A' }.AsReadOnly();
            ContentPermissionsResource resource = createWithNodeId
                ? new ContentPermissionsResource(content, nodeId, permissions)
                : new ContentPermissionsResource(content, permissions);
            return new AuthorizationHandlerContext(new List<IAuthorizationRequirement> { requirement }, user, resource);
        }

        private static IContent CreateContent(int nodeId)
        {
            ContentType contentType = ContentTypeBuilder.CreateBasicContentType();
            return ContentBuilder.CreateBasicContent(contentType, nodeId);
        }

        private ContentPermissionsResourceHandler CreateHandler(int nodeId, string[] permissionsForPath)
        {
            Mock<IBackOfficeSecurityAccessor> mockBackOfficeSecurityAccessor = CreateMockBackOfficeSecurityAccessor();
            ContentPermissions contentPermissions = CreateContentPermissions(nodeId, permissionsForPath);
            return new ContentPermissionsResourceHandler(mockBackOfficeSecurityAccessor.Object, contentPermissions);
        }

        private static Mock<IBackOfficeSecurityAccessor> CreateMockBackOfficeSecurityAccessor()
        {
            User user = CreateUser();
            var mockBackOfficeSecurity = new Mock<IBackOfficeSecurity>();
            mockBackOfficeSecurity.SetupGet(x => x.CurrentUser).Returns(user);
            var mockBackOfficeSecurityAccessor = new Mock<IBackOfficeSecurityAccessor>();
            mockBackOfficeSecurityAccessor.Setup(x => x.BackOfficeSecurity).Returns(mockBackOfficeSecurity.Object);
            return mockBackOfficeSecurityAccessor;
        }

        private static User CreateUser() =>
            new UserBuilder()
                .Build();

        private static ContentPermissions CreateContentPermissions(int nodeId, string[] permissionsForPath)
        {
            var mockUserService = new Mock<IUserService>();

            mockUserService
                .Setup(x => x.GetPermissionsForPath(It.IsAny<IUser>(), It.Is<string>(y => y == $"{Constants.System.RootString},{nodeId.ToInvariantString()}")))
                .Returns(new EntityPermissionSet(nodeId, new EntityPermissionCollection(new List<EntityPermission> { new EntityPermission(1, nodeId, permissionsForPath) })));

            var mockContentService = new Mock<IContentService>();
            mockContentService
                .Setup(x => x.GetById(It.Is<int>(y => y == nodeId)))
                .Returns(CreateContent(nodeId));

            var mockEntityService = new Mock<IEntityService>();
            return new ContentPermissions(mockUserService.Object, mockContentService.Object, mockEntityService.Object, AppCaches.Disabled);
        }
    }
}
