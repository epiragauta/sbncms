using System;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Sbn.Cms.Core;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.Hosting;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Web.Mvc;
using Sbn.Cms.Web.Common.Controllers;
using Sbn.Cms.Web.Common.Routing;
using Sbn.Cms.Web.Website.Collections;
using Sbn.Extensions;

namespace Sbn.Cms.Web.Website.Routing
{
    /// <summary>
    /// Creates routes for surface controllers
    /// </summary>
    public sealed class FrontEndRoutes : IAreaRoutes
    {
        private readonly GlobalSettings _globalSettings;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IRuntimeState _runtimeState;
        private readonly SurfaceControllerTypeCollection _surfaceControllerTypeCollection;
        private readonly SbnApiControllerTypeCollection _apiControllers;
        private readonly string _sbnPathSegment;

        /// <summary>
        /// Initializes a new instance of the <see cref="FrontEndRoutes"/> class.
        /// </summary>
        public FrontEndRoutes(
            IOptions<GlobalSettings> globalSettings,
            IHostingEnvironment hostingEnvironment,
            IRuntimeState runtimeState,
            SurfaceControllerTypeCollection surfaceControllerTypeCollection,
            SbnApiControllerTypeCollection apiControllers)
        {
            _globalSettings = globalSettings.Value;
            _hostingEnvironment = hostingEnvironment;
            _runtimeState = runtimeState;
            _surfaceControllerTypeCollection = surfaceControllerTypeCollection;
            _apiControllers = apiControllers;
            _sbnPathSegment = _globalSettings.GetSbnMvcArea(_hostingEnvironment);
        }

        /// <inheritdoc/>
        public void CreateRoutes(IEndpointRouteBuilder endpoints)
        {
            if (_runtimeState.Level != RuntimeLevel.Run)
            {
                return;
            }

            AutoRouteSurfaceControllers(endpoints);
            AutoRouteFrontEndApiControllers(endpoints);
        }

        /// <summary>
        /// Auto-routes all front-end surface controllers
        /// </summary>
        private void AutoRouteSurfaceControllers(IEndpointRouteBuilder endpoints)
        {
            foreach (Type controller in _surfaceControllerTypeCollection)
            {
                // exclude front-end api controllers
                PluginControllerMetadata meta = PluginController.GetMetadata(controller);

                endpoints.MapSbnSurfaceRoute(
                    meta.ControllerType,
                    _sbnPathSegment,
                    meta.AreaName);
            }
        }

        /// <summary>
        /// Auto-routes all front-end api controllers
        /// </summary>
        private void AutoRouteFrontEndApiControllers(IEndpointRouteBuilder endpoints)
        {
            foreach (Type controller in _apiControllers)
            {
                PluginControllerMetadata meta = PluginController.GetMetadata(controller);

                // exclude back-end api controllers
                if (meta.IsBackOffice)
                {
                    continue;
                }

                endpoints.MapSbnApiRoute(
                    meta.ControllerType,
                    _sbnPathSegment,
                    meta.AreaName,
                    meta.IsBackOffice,
                    defaultAction: string.Empty); // no default action (this is what we had before)
            }
        }
    }
}
