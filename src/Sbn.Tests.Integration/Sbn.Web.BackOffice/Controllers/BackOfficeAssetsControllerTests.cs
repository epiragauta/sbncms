// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using Sbn.Cms.Tests.Integration.TestServerTest;
using Sbn.Cms.Web.BackOffice.Controllers;

namespace Sbn.Cms.Tests.Integration.Sbn.Web.BackOffice.Controllers
{
    [TestFixture]
    public class BackOfficeAssetsControllerTests : SbnTestServerTestBase
    {
        [Test]
        public async Task EnsureSuccessStatusCode()
        {
            // Arrange
            string url = PrepareApiControllerUrl<BackOfficeAssetsController>(x => x.GetSupportedLocales());

            // Act
            HttpResponseMessage response = await Client.GetAsync(url);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
