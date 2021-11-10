// Copyright (c) Sbn.
// See LICENSE for more details.

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Web.Common.AspNetCore;
using IHostingEnvironment = Sbn.Cms.Core.Hosting.IHostingEnvironment;

namespace Sbn.Cms.Tests.Common.Testing
{
    public class TestHostingEnvironment : AspNetCoreHostingEnvironment, IHostingEnvironment
    {
        public TestHostingEnvironment(
            IOptionsMonitor<HostingSettings> hostingSettings,
            IOptionsMonitor<WebRoutingSettings> webRoutingSettings,
            IWebHostEnvironment webHostEnvironment)
            : base(null, hostingSettings, webRoutingSettings, webHostEnvironment)
        {
        }

        // override
        string IHostingEnvironment.ApplicationId { get; } = "TestApplication";


        /// <summary>
        /// Gets a value indicating whether we are hosted.
        /// </summary>
        /// <remarks>
        /// This is specifically used by IOHelper and we want this to return false so that the root path is manually
        /// calculated which is what we want for tests.
        /// </remarks>
        bool IHostingEnvironment.IsHosted { get; } = false;
    }
}
