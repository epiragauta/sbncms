using Microsoft.AspNetCore.Authorization;
using Sbn.Cms.Core.Hosting;
using Sbn.Cms.Web.BackOffice.Controllers;
using Sbn.Cms.Web.Common.Attributes;
using Sbn.Cms.Web.Common.Authorization;
using Constants = Sbn.Cms.Core.Constants;

namespace Sbn.Cms.Web.BackOffice.Profiling
{
    /// <summary>
    /// The API controller used to display the state of the web profiler
    /// </summary>
    [PluginController(Constants.Web.Mvc.BackOfficeApiArea)]
    [Authorize(Policy = AuthorizationPolicies.SectionAccessSettings)]
    public class WebProfilingController : SbnAuthorizedJsonController
    {
        private readonly IHostingEnvironment _hosting;

        public WebProfilingController(IHostingEnvironment hosting)
        {
            _hosting = hosting;
        }

        public object GetStatus()
        {
            return new
            {
                Enabled = _hosting.IsDebugMode
            };
        }
    }}
