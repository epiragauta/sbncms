using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Sbn.Cms.Core.DependencyInjection;
using Sbn.Cms.Core.Security;
using Sbn.Cms.Tests.Integration.Testing;
using Sbn.Extensions;

namespace Sbn.Tests.Integration.Sbn.Web.Common
{
    [TestFixture]
    public class MembersServiceCollectionExtensionsTests : SbnIntegrationTest
    {
        protected override void CustomTestSetup(ISbnBuilder builder) => builder.AddMembersIdentity();

        [Test]
        public void AddMembersIdentity_ExpectMembersUserStoreResolvable()
        {
            IUserStore<MemberIdentityUser> userStore = Services.GetService<IUserStore<MemberIdentityUser>>();

            Assert.IsNotNull(userStore);
            Assert.AreEqual(typeof(MemberUserStore), userStore.GetType());
        }

        [Test]
        public void AddMembersIdentity_ExpectMembersUserManagerResolvable()
        {
            IMemberManager userManager = Services.GetService<IMemberManager>();

            Assert.NotNull(userManager);
        }
    }
}
