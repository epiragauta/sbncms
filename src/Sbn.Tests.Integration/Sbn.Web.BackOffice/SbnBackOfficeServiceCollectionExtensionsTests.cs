// Copyright (c) Sbn.
// See LICENSE for more details.

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Sbn.Cms.Core.DependencyInjection;
using Sbn.Cms.Core.Security;
using Sbn.Cms.Tests.Integration.Testing;
using Sbn.Extensions;

namespace Sbn.Cms.Tests.Integration.Sbn.Web.BackOffice
{
    [TestFixture]
    public class SbnBackOfficeServiceCollectionExtensionsTests : SbnIntegrationTest
    {
        protected override void CustomTestSetup(ISbnBuilder builder) => builder.AddBackOfficeIdentity();

        [Test]
        public void AddSbnBackOfficeIdentity_ExpectBackOfficeUserStoreResolvable()
        {
            IUserStore<BackOfficeIdentityUser> userStore = Services.GetService<IUserStore<BackOfficeIdentityUser>>();

            Assert.IsNotNull(userStore);
            Assert.AreEqual(typeof(BackOfficeUserStore), userStore.GetType());
        }

        [Test]
        public void AddSbnBackOfficeIdentity_ExpectBackOfficeClaimsPrincipalFactoryResolvable()
        {
            IUserClaimsPrincipalFactory<BackOfficeIdentityUser> principalFactory = Services.GetService<IUserClaimsPrincipalFactory<BackOfficeIdentityUser>>();

            Assert.IsNotNull(principalFactory);
            Assert.AreEqual(typeof(BackOfficeClaimsPrincipalFactory), principalFactory.GetType());
        }

        [Test]
        public void AddSbnBackOfficeIdentity_ExpectBackOfficeUserManagerResolvable()
        {
            IBackOfficeUserManager userManager = Services.GetService<IBackOfficeUserManager>();

            Assert.NotNull(userManager);
        }
    }
}
