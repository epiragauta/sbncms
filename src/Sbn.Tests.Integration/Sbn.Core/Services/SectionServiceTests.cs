// Copyright (c) Sbn.
// See LICENSE for more details.

using System;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.Models.Membership;
using Sbn.Cms.Core.Scoping;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Tests.Common.Testing;
using Sbn.Cms.Tests.Integration.Testing;

namespace Sbn.Cms.Tests.Integration.Sbn.Core.Services
{
    /// <summary>
    /// Tests covering the SectionService
    /// </summary>
    [TestFixture]
    [SbnTest(Database = SbnTestOptions.Database.NewSchemaPerTest)]
    public class SectionServiceTests : SbnIntegrationTest
    {
        private ISectionService SectionService => GetRequiredService<ISectionService>();

        private IUserService UserService => GetRequiredService<IUserService>();

        [Test]
        public void SectionService_Can_Get_Allowed_Sections_For_User()
        {
            // Arrange
            IUser user = CreateTestUser();

            // Act
            var result = SectionService.GetAllowedSections(user.Id).ToList();

            // Assert
            Assert.AreEqual(3, result.Count);
        }

        private IUser CreateTestUser()
        {
            using IScope scope = ScopeProvider.CreateScope(autoComplete: true);
            using IDisposable _ = scope.Notifications.Suppress();

            var globalSettings = new GlobalSettings();
            var user = new User(globalSettings)
            {
                Name = "Test user",
                Username = "testUser",
                Email = "testuser@test.com",
            };
            UserService.Save(user);

            var userGroupA = new UserGroup(ShortStringHelper)
            {
                Alias = "GroupA",
                Name = "Group A"
            };
            userGroupA.AddAllowedSection("media");
            userGroupA.AddAllowedSection("settings");

            // TODO: This is failing the test
            UserService.Save(userGroupA, new[] { user.Id });

            var userGroupB = new UserGroup(ShortStringHelper)
            {
                Alias = "GroupB",
                Name = "Group B"
            };
            userGroupB.AddAllowedSection("settings");
            userGroupB.AddAllowedSection("member");
            UserService.Save(userGroupB, new[] { user.Id });

            return UserService.GetUserById(user.Id);
        }
    }
}
