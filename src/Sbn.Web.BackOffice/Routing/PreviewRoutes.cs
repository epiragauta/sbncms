using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Sbn.Cms.Core;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.Hosting;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Web.BackOffice.Controllers;
using Sbn.Cms.Web.BackOffice.SignalR;
using Sbn.Cms.Web.Common.Routing;
using Sbn.Extensions;
using Constants = Sbn.Cms.Core.Constants;

namespace Sbn.Cms.Web.BackOffice.Routing
{
    /// <summary>
    /// Creates routes for the preview hub
    /// </summary>
    public sealed class PreviewRoutes : IAreaRoutes
    {
        private readonly IRuntimeState _runtimeState;
        private readonly string _sbnPathSegment;

        public PreviewRoutes(
            IOptions<GlobalSettings> globalSettings,
            IHostingEnvironment hostingEnvironment,
            IRuntimeState runtimeState)
        {
            _runtimeState = runtimeState;
            _sbnPathSegment = globalSettings.Value.GetSbnMvcArea(hostingEnvironment);
        }

        public void CreateRoutes(IEndpointRouteBuilder endpoints)
        {
            switch (_runtimeState.Level)
            {
                case RuntimeLevel.Install:
                case RuntimeLevel.Upgrade:
                case RuntimeLevel.Run:
                    endpoints.MapHub<PreviewHub>(GetPreviewHubRoute());
                    endpoints.MapSbnRoute<PreviewController>(_sbnPathSegment, Constants.Web.Mvc.BackOfficeArea, null);
                    break;
                case RuntimeLevel.BootFailed:
                case RuntimeLevel.Unknown:
                case RuntimeLevel.Boot:
                    break;
            }
        }

        /// <summary>
        /// Returns the path to the signalR hub used for preview
        /// </summary>
        /// <returns>Path to signalR hub</returns>
        public string GetPreviewHubRoute()
        {
            return $"/{_sbnPathSegment}/{nameof(PreviewHub)}";
        }
    }
}
