// Copyright (c) Sbn.
// See LICENSE for more details.

using Microsoft.AspNetCore.DataProtection;
using NUnit.Framework;
using Sbn.Cms.Web.Common.Exceptions;
using Sbn.Cms.Web.Common.Filters;
using Sbn.Cms.Web.Common.Security;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Web.Common.Filters
{
    [TestFixture]
    public class ValidateSbnFormRouteStringFilterTests
    {
        private IDataProtectionProvider DataProtectionProvider { get; } = new EphemeralDataProtectionProvider();

        [Test]
        public void Validate_Route_String()
        {
            var filter = new ValidateSbnFormRouteStringAttribute.ValidateSbnFormRouteStringFilter(DataProtectionProvider);

            Assert.Throws<HttpSbnFormRouteStringException>(() => filter.ValidateRouteString(null, null, null, null));

            const string ControllerName = "Test";
            const string ControllerAction = "Index";
            const string Area = "MyArea";
            var validUfprt = EncryptionHelper.CreateEncryptedRouteString(DataProtectionProvider, ControllerName, ControllerAction, Area);

            var invalidUfprt = validUfprt + "z";
            Assert.Throws<HttpSbnFormRouteStringException>(() => filter.ValidateRouteString(invalidUfprt, null, null, null));

            Assert.Throws<HttpSbnFormRouteStringException>(() => filter.ValidateRouteString(validUfprt, ControllerName, ControllerAction, "doesntMatch"));
            Assert.Throws<HttpSbnFormRouteStringException>(() => filter.ValidateRouteString(validUfprt, ControllerName, ControllerAction, null));
            Assert.Throws<HttpSbnFormRouteStringException>(() => filter.ValidateRouteString(validUfprt, ControllerName, "doesntMatch", Area));
            Assert.Throws<HttpSbnFormRouteStringException>(() => filter.ValidateRouteString(validUfprt, ControllerName, null, Area));
            Assert.Throws<HttpSbnFormRouteStringException>(() => filter.ValidateRouteString(validUfprt, "doesntMatch", ControllerAction, Area));
            Assert.Throws<HttpSbnFormRouteStringException>(() => filter.ValidateRouteString(validUfprt, null, ControllerAction, Area));

            Assert.DoesNotThrow(() => filter.ValidateRouteString(validUfprt, ControllerName, ControllerAction, Area));
            Assert.DoesNotThrow(() => filter.ValidateRouteString(validUfprt, ControllerName.ToLowerInvariant(), ControllerAction.ToLowerInvariant(), Area.ToLowerInvariant()));
        }
    }
}
