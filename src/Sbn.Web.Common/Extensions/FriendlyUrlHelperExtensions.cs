using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Sbn.Cms.Core.Web;
using Sbn.Cms.Web.Common.DependencyInjection;

namespace Sbn.Extensions
{
    public static class FriendlyUrlHelperExtensions
    {

        private static ISbnContext SbnContext { get; } =
            StaticServiceProvider.Instance.GetRequiredService<ISbnContextAccessor>().GetRequiredSbnContext();

        private static IDataProtectionProvider DataProtectionProvider { get; } =
            StaticServiceProvider.Instance.GetRequiredService<IDataProtectionProvider>();
        /// <summary>
        /// Generates a URL based on the current Sbn URL with a custom query string that will route to the specified SurfaceController
        /// </summary>
        /// <param name="url"></param>
        /// <param name="action"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public static string SurfaceAction(this IUrlHelper url, string action, string controllerName)
            => UrlHelperExtensions.SurfaceAction(url, SbnContext, DataProtectionProvider, action, controllerName);

        /// <summary>
        /// Generates a URL based on the current Sbn URL with a custom query string that will route to the specified SurfaceController
        /// </summary>
        /// <param name="url"></param>
        /// <param name="action"></param>
        /// <param name="controllerName"></param>
        /// <param name="additionalRouteVals"></param>
        /// <returns></returns>
        public static string SurfaceAction(this IUrlHelper url, string action, string controllerName, object additionalRouteVals)
            => UrlHelperExtensions.SurfaceAction(url, SbnContext, DataProtectionProvider, action, controllerName, additionalRouteVals);

        /// <summary>
        /// Generates a URL based on the current Sbn URL with a custom query string that will route to the specified SurfaceController
        /// </summary>
        /// <param name="url"></param>
        /// <param name="action"></param>
        /// <param name="controllerName"></param>
        /// <param name="area"></param>
        /// <param name="additionalRouteVals"></param>
        /// <returns></returns>
        public static string SurfaceAction(this IUrlHelper url, string action, string controllerName, string area, object additionalRouteVals)
            => UrlHelperExtensions.SurfaceAction(url, SbnContext, DataProtectionProvider, action, controllerName, area, additionalRouteVals);
    }
}
