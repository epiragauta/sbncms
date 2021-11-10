using System;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Sbn.Extensions
{
    public static class EndpointRouteBuilderExtensions
    {
        /// <summary>
        /// Used to map Sbn controllers consistently
        /// </summary>
        public static void MapSbnRoute(
            this IEndpointRouteBuilder endpoints,
            Type controllerType,
            string rootSegment,
            string areaName,
            string prefixPathSegment,
            string defaultAction = "Index",
            bool includeControllerNameInRoute = true,
            object constraints = null)
        {
            var controllerName = ControllerExtensions.GetControllerName(controllerType);

            // build the route pattern
            var pattern = new StringBuilder(rootSegment);
            if (!prefixPathSegment.IsNullOrWhiteSpace())
            {
                pattern.Append('/').Append(prefixPathSegment);
            }

            if (includeControllerNameInRoute)
            {
                pattern.Append('/').Append(controllerName);
            }

            pattern.Append("/{action}/{id?}");

            var defaults = defaultAction.IsNullOrWhiteSpace()
                ? (object)new { controller = controllerName }
                : new { controller = controllerName, action = defaultAction };

            if (areaName.IsNullOrWhiteSpace())
            {
                endpoints.MapControllerRoute(

                    // named consistently
                    $"sbn-{areaName}-{controllerName}".ToLowerInvariant(),
                    pattern.ToString().ToLowerInvariant(),
                    defaults,
                    constraints);
            }
            else
            {
                endpoints.MapAreaControllerRoute(

                    // named consistently
                    $"sbn-{areaName}-{controllerName}".ToLowerInvariant(),
                    areaName,
                    pattern.ToString().ToLowerInvariant(),
                    defaults,
                    constraints);
            }
        }

        /// <summary>
        /// Used to map Sbn controllers consistently
        /// </summary>
        /// <typeparam name="T">The <see cref="ControllerBase"/> type to route</typeparam>
        public static void MapSbnRoute<T>(
            this IEndpointRouteBuilder endpoints,
            string rootSegment,
            string areaName,
            string prefixPathSegment,
            string defaultAction = "Index",
            bool includeControllerNameInRoute = true,
            object constraints = null)
            where T : ControllerBase
            => endpoints.MapSbnRoute(typeof(T), rootSegment, areaName, prefixPathSegment, defaultAction, includeControllerNameInRoute, constraints);

        /// <summary>
        /// Used to map controllers as Sbn API routes consistently
        /// </summary>
        /// <typeparam name="T">The <see cref="ControllerBase"/> type to route</typeparam>
        public static void MapSbnApiRoute<T>(
            this IEndpointRouteBuilder endpoints,
            string rootSegment,
            string areaName,
            bool isBackOffice,
            string defaultAction = "Index",
            object constraints = null)
            where T : ControllerBase
            => endpoints.MapSbnApiRoute(typeof(T), rootSegment, areaName, isBackOffice, defaultAction, constraints);

        /// <summary>
        /// Used to map controllers as Sbn API routes consistently
        /// </summary>
        public static void MapSbnApiRoute(
            this IEndpointRouteBuilder endpoints,
            Type controllerType,
            string rootSegment,
            string areaName,
            bool isBackOffice,
            string defaultAction = "Index",
            object constraints = null)
        {
            string prefixPathSegment = isBackOffice
                ? areaName.IsNullOrWhiteSpace()
                    ? $"{Cms.Core.Constants.Web.Mvc.BackOfficePathSegment}/Api"
                    : $"{Cms.Core.Constants.Web.Mvc.BackOfficePathSegment}/{areaName}"
                : areaName.IsNullOrWhiteSpace()
                    ? "Api"
                    : areaName;

            endpoints.MapSbnRoute(
                           controllerType,
                           rootSegment,
                           areaName,
                           prefixPathSegment,
                           defaultAction,
                           true,
                           constraints);
        }

        public static void MapSbnSurfaceRoute(
            this IEndpointRouteBuilder endpoints,
            Type controllerType,
            string rootSegment,
            string areaName,
            string defaultAction = "Index",
            bool includeControllerNameInRoute = true,
            object constraints = null)
        {
            // If there is an area name it's a plugin controller, and we should use the area name instead of surface
            string prefixPathSegment = areaName.IsNullOrWhiteSpace() ? "Surface" : areaName;

            endpoints.MapSbnRoute(
                controllerType,
                rootSegment,
                areaName,
                prefixPathSegment,
                defaultAction,
                includeControllerNameInRoute,
                constraints);
        }
    }
}
