// Copyright (c) Sbn.
// See LICENSE for more details.

using AutoFixture.NUnit3;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Tests.UnitTests.AutoFixture;
using Sbn.Cms.Web.Common.AspNetCore;
using Sbn.Extensions;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Core.Configuration.Models
{
    [TestFixture]
    public class GlobalSettingsTests
    {
        [InlineAutoMoqData("~/sbn", "/", "sbn")]
        [InlineAutoMoqData("~/sbn", "/MyVirtualDir", "sbn")]
        [InlineAutoMoqData("~/customPath", "/MyVirtualDir/", "custompath")]
        [InlineAutoMoqData("~/some-wacky/nestedPath", "/MyVirtualDir", "some-wacky-nestedpath")]
        [InlineAutoMoqData("~/some-wacky/nestedPath", "/MyVirtualDir/NestedVDir/", "some-wacky-nestedpath")]
        public void Sbn_Mvc_Area(
            string path,
            string rootPath,
            string outcome,
            [Frozen] IOptionsMonitor<HostingSettings> hostingSettings,
            AspNetCoreHostingEnvironment hostingEnvironment)
        {
            hostingSettings.CurrentValue.ApplicationVirtualPath = rootPath;

            var globalSettings = new GlobalSettings { SbnPath = path };

            Assert.AreEqual(outcome, globalSettings.GetSbnMvcAreaNoCache(hostingEnvironment));
        }
    }
}
