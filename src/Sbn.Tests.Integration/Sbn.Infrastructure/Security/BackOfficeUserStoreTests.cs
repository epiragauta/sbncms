using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.Mapping;
using Sbn.Cms.Core.Models.Membership;
using Sbn.Cms.Core.Scoping;
using Sbn.Cms.Core.Security;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Tests.Common.Testing;
using Sbn.Cms.Tests.Integration.Testing;

namespace Sbn.Cms.Tests.Integration.Sbn.Infrastructure.Security
{
    [TestFixture]
    [SbnTest(Database = SbnTestOptions.Database.NewSchemaPerTest)]
    public class BackOfficeUserStoreTests : SbnIntegrationTest
    {
        private IUserService UserService => GetRequiredService<IUserService>();
        private IEntityService EntityService => GetRequiredService<IEntityService>();
        private IExternalLoginService ExternalLoginService => GetRequiredService<IExternalLoginService>();
        private ISbnMapper SbnMapper => GetRequiredService<ISbnMapper>();
        private ILocalizedTextService TextService => GetRequiredService<ILocalizedTextService>();

        private BackOfficeUserStore GetUserStore()
            => new BackOfficeUserStore(
                    ScopeProvider,
                    UserService,
                    EntityService,
                    ExternalLoginService,
                    Options.Create(GlobalSettings),
                    SbnMapper,
                    new BackOfficeErrorDescriber(TextService),
                    AppCaches);

        [Test]
        public async Task Can_Persist_Is_Approved()
        {
            var userStore = GetUserStore();
            var user = new BackOfficeIdentityUser(GlobalSettings, 1, new List<IReadOnlyUserGroup>())
            {
                Name = "Test",
                Email = "test@test.com",
                UserName = "test@test.com"
            };
            IdentityResult createResult = await userStore.CreateAsync(user);
            Assert.IsTrue(createResult.Succeeded);
            Assert.IsFalse(user.IsApproved);

            // update
            user.IsApproved = true;
            var saveResult = await userStore.UpdateAsync(user);
            Assert.IsTrue(saveResult.Succeeded);
            Assert.IsTrue(user.IsApproved);

            // get get
            user = await userStore.FindByIdAsync(user.Id);
            Assert.IsTrue(user.IsApproved);
        }
    }
}
