using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Sbn.Cms.Core;
using Sbn.Cms.Core.Configuration;
using Sbn.Cms.Core.Hosting;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Models.PublishedContent;
using Sbn.Cms.Core.PropertyEditors.ValueConverters;
using Sbn.Cms.Core.Web;
using Sbn.Cms.Core.WebAssets;
using Sbn.Cms.Web.Common.Controllers;
using Sbn.Cms.Web.Common.Security;

namespace Sbn.Extensions
{
    public static class UrlHelperExtensions
    {
        /// <summary>
        /// Return the back office url if the back office is installed
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetBackOfficeUrl(this IUrlHelper url)
        {
            var backOfficeControllerType = Type.GetType("Sbn.Web.BackOffice.Controllers");
            if (backOfficeControllerType == null) return "/"; // this would indicate that the installer is installed without the back office
            return url.Action("Default", ControllerExtensions.GetControllerName(backOfficeControllerType), new { area = Cms.Core.Constants.Web.Mvc.BackOfficeApiArea });
        }

        /// <summary>
        /// Return the Url for a Web Api service
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="sbnApiControllerTypeCollection"></param>
        /// <param name="actionName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetSbnApiService<T>(this IUrlHelper url, SbnApiControllerTypeCollection sbnApiControllerTypeCollection, string actionName, object id = null)
            where T : SbnApiController
        {
            return url.GetSbnApiService(sbnApiControllerTypeCollection, actionName, typeof(T), id);
        }

        public static string GetSbnApiService<T>(this IUrlHelper url, SbnApiControllerTypeCollection sbnApiControllerTypeCollection, Expression<Func<T, object>> methodSelector)
            where T : SbnApiController
        {
            var method = ExpressionHelper.GetMethodInfo(methodSelector);
            var methodParams = ExpressionHelper.GetMethodParams(methodSelector);
            if (method == null)
            {
                throw new MissingMethodException("Could not find the method " + methodSelector + " on type " + typeof(T) + " or the result ");
            }

            if (methodParams.Any() == false)
            {
                return url.GetSbnApiService<T>(sbnApiControllerTypeCollection, method.Name);
            }
            return url.GetSbnApiService<T>(sbnApiControllerTypeCollection, method.Name, methodParams.Values.First());
        }

        /// <summary>
        /// Return the Url for a Web Api service
        /// </summary>
        /// <param name="url"></param>
        /// <param name="sbnApiControllerTypeCollection"></param>
        /// <param name="actionName"></param>
        /// <param name="apiControllerType"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetSbnApiService(this IUrlHelper url, SbnApiControllerTypeCollection sbnApiControllerTypeCollection, string actionName, Type apiControllerType, object id = null)
        {
            if (actionName == null) throw new ArgumentNullException(nameof(actionName));
            if (string.IsNullOrWhiteSpace(actionName)) throw new ArgumentException("Value can't be empty or consist only of white-space characters.", nameof(actionName));
            if (apiControllerType == null) throw new ArgumentNullException(nameof(apiControllerType));

            var area = "";

            var apiController = sbnApiControllerTypeCollection.SingleOrDefault(x => x == apiControllerType);
            if (apiController == null)
                throw new InvalidOperationException("Could not find the sbn api controller of type " + apiControllerType.FullName);
            var metaData = PluginController.GetMetadata(apiController);
            if (metaData.AreaName.IsNullOrWhiteSpace() == false)
            {
                //set the area to the plugin area
                area = metaData.AreaName;
            }
            return url.GetSbnApiService(actionName, ControllerExtensions.GetControllerName(apiControllerType), area, id);
        }

        /// <summary>
        /// Return the Url for a Web Api service
        /// </summary>
        /// <param name="url"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetSbnApiService(this IUrlHelper url, string actionName, string controllerName, object id = null)
        {
            return url.GetSbnApiService(actionName, controllerName, "", id);
        }

        /// <summary>
        /// Return the Url for a Web Api service
        /// </summary>
        /// <param name="url"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="area"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetSbnApiService(this IUrlHelper url, string actionName, string controllerName, string area, object id = null)
        {
            if (actionName == null) throw new ArgumentNullException(nameof(actionName));
            if (string.IsNullOrWhiteSpace(actionName)) throw new ArgumentException("Value can't be empty or consist only of white-space characters.", nameof(actionName));
            if (controllerName == null) throw new ArgumentNullException(nameof(controllerName));
            if (string.IsNullOrWhiteSpace(controllerName)) throw new ArgumentException("Value can't be empty or consist only of white-space characters.", nameof(controllerName));

            if (area.IsNullOrWhiteSpace())
            {
                if (id == null)
                {
                    return url.Action(actionName, controllerName);
                }
                else
                {
                    return url.Action(actionName, controllerName, new { id = id });
                }
            }
            else
            {
                if (id == null)
                {
                    return url.Action(actionName, controllerName, new { area = area });
                }
                else
                {
                    return url.Action(actionName, controllerName, new { area = area, id = id });
                }
            }
        }

        /// <summary>
        /// Return the Base Url (not including the action) for a Web Api service
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="sbnApiControllerTypeCollection"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public static string GetSbnApiServiceBaseUrl<T>(this IUrlHelper url, SbnApiControllerTypeCollection sbnApiControllerTypeCollection, string actionName)
            where T : SbnApiController
        {
            return url.GetSbnApiService<T>(sbnApiControllerTypeCollection, actionName).TrimEnd(actionName);
        }

        public static string GetSbnApiServiceBaseUrl<T>(this IUrlHelper url, SbnApiControllerTypeCollection sbnApiControllerTypeCollection, Expression<Func<T, object>> methodSelector)
            where T : SbnApiController
        {
            var method = ExpressionHelper.GetMethodInfo(methodSelector);
            if (method == null)
            {
                throw new MissingMethodException("Could not find the method " + methodSelector + " on type " + typeof(T) + " or the result ");
            }
            return url.GetSbnApiService<T>(sbnApiControllerTypeCollection, method.Name).TrimEnd(method.Name);
        }

        /// <summary>
        /// Return the Url for an action with a cache-busting hash appended
        /// </summary>
        /// <param name="url"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeVals"></param>
        /// <returns></returns>
        public static string GetUrlWithCacheBust(this IUrlHelper url, string actionName, string controllerName, RouteValueDictionary routeVals,
            IHostingEnvironment hostingEnvironment, ISbnVersion sbnVersion, IRuntimeMinifier runtimeMinifier)
        {
            var applicationJs = url.Action(actionName, controllerName, routeVals);
            applicationJs = applicationJs + "?umb__rnd=" + GetCacheBustHash(hostingEnvironment, sbnVersion, runtimeMinifier);
            return applicationJs;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static string GetCacheBustHash(IHostingEnvironment hostingEnvironment, ISbnVersion sbnVersion, IRuntimeMinifier runtimeMinifier)
        {
            //make a hash of sbn and client dependency version
            //in case the user bypasses the installer and just bumps the web.config or client dependency config

            //if in debug mode, always burst the cache
            if (hostingEnvironment.IsDebugMode)
            {
                return DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture).GenerateHash();
            }

            var version = sbnVersion.SemanticVersion.ToSemanticString();
            return $"{version}.{runtimeMinifier.CacheBuster}".GenerateHash();
        }

        public static IHtmlContent GetCropUrl(this IUrlHelper urlHelper, IPublishedContent mediaItem, string cropAlias, bool htmlEncode = true, UrlMode urlMode = UrlMode.Default)
        {
            if (mediaItem == null)
            {
                return HtmlString.Empty;
            }

            var url = mediaItem.GetCropUrl(cropAlias: cropAlias, useCropDimensions: true, urlMode: urlMode);
            return CreateHtmlString(url, htmlEncode);
        }

        private static IHtmlContent CreateHtmlString(string url, bool htmlEncode) => htmlEncode ? new HtmlString(HttpUtility.HtmlEncode(url)) : new HtmlString(url);

        public static IHtmlContent GetCropUrl(this IUrlHelper urlHelper, IPublishedContent mediaItem, string propertyAlias, string cropAlias, bool htmlEncode = true, UrlMode urlMode = UrlMode.Default)
        {
            if (mediaItem == null)
            {
                return HtmlString.Empty;
            }

            var url = mediaItem.GetCropUrl(propertyAlias: propertyAlias, cropAlias: cropAlias, useCropDimensions: true, urlMode: urlMode);
            return CreateHtmlString(url, htmlEncode);
        }

        public static IHtmlContent GetCropUrl(this IUrlHelper urlHelper,
            IPublishedContent mediaItem,
            int? width = null,
            int? height = null,
            string propertyAlias = Constants.Conventions.Media.File,
            string cropAlias = null,
            int? quality = null,
            ImageCropMode? imageCropMode = null,
            ImageCropAnchor? imageCropAnchor = null,
            bool preferFocalPoint = false,
            bool useCropDimensions = false,
            bool cacheBuster = true,
            string furtherOptions = null,
            bool htmlEncode = true,
            UrlMode urlMode = UrlMode.Default)
        {
            if (mediaItem == null)
            {
                return HtmlString.Empty;
            }

            var url = mediaItem.GetCropUrl(width, height, propertyAlias, cropAlias, quality, imageCropMode,
                imageCropAnchor, preferFocalPoint, useCropDimensions, cacheBuster, furtherOptions, urlMode);

            return CreateHtmlString(url, htmlEncode);
        }

        public static IHtmlContent GetCropUrl(this IUrlHelper urlHelper,
            ImageCropperValue imageCropperValue,
            string cropAlias,
            int? width = null,
            int? height = null,
            int? quality = null,
            ImageCropMode? imageCropMode = null,
            ImageCropAnchor? imageCropAnchor = null,
            bool preferFocalPoint = false,
            bool useCropDimensions = true,
            string cacheBusterValue = null,
            string furtherOptions = null,
            bool htmlEncode = true)
        {
            if (imageCropperValue == null) return HtmlString.Empty;

            var imageUrl = imageCropperValue.Src;
            var url = imageUrl.GetCropUrl(imageCropperValue, width, height, cropAlias, quality, imageCropMode,
                imageCropAnchor, preferFocalPoint, useCropDimensions, cacheBusterValue, furtherOptions);

            return CreateHtmlString(url, htmlEncode);
        }

        /// <summary>
        /// Generates a URL based on the current Sbn URL with a custom query string that will route to the specified SurfaceController
        /// </summary>
        /// <param name="url"></param>
        /// <param name="action"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public static string SurfaceAction(this IUrlHelper url, ISbnContext sbnContext, IDataProtectionProvider dataProtectionProvider,string action, string controllerName)
        {
            return url.SurfaceAction(sbnContext, dataProtectionProvider, action, controllerName, null);
        }

        /// <summary>
        /// Generates a URL based on the current Sbn URL with a custom query string that will route to the specified SurfaceController
        /// </summary>
        /// <param name="url"></param>
        /// <param name="action"></param>
        /// <param name="controllerName"></param>
        /// <param name="additionalRouteVals"></param>
        /// <returns></returns>
        public static string SurfaceAction(this IUrlHelper url, ISbnContext sbnContext, IDataProtectionProvider dataProtectionProvider,string action, string controllerName, object additionalRouteVals)
        {
            return url.SurfaceAction(sbnContext, dataProtectionProvider, action, controllerName, "", additionalRouteVals);
        }

        /// <summary>
        /// Generates a URL based on the current Sbn URL with a custom query string that will route to the specified SurfaceController
        /// </summary>
        /// <param name="url"></param>
        /// <param name="action"></param>
        /// <param name="controllerName"></param>
        /// <param name="area"></param>
        /// <param name="additionalRouteVals"></param>
        /// <returns></returns>
        public static string SurfaceAction(this IUrlHelper url, ISbnContext sbnContext, IDataProtectionProvider dataProtectionProvider, string action, string controllerName, string area, object additionalRouteVals)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            if (string.IsNullOrEmpty(action)) throw new ArgumentException("Value can't be empty.", nameof(action));
            if (controllerName == null) throw new ArgumentNullException(nameof(controllerName));
            if (string.IsNullOrEmpty(controllerName)) throw new ArgumentException("Value can't be empty.", nameof(controllerName));

            var encryptedRoute = EncryptionHelper.CreateEncryptedRouteString(dataProtectionProvider, controllerName, action, area, additionalRouteVals);

            var result = sbnContext.OriginalRequestUrl.AbsolutePath.EnsureEndsWith('?') + "ufprt=" + encryptedRoute;
            return result;
        }
    }
}
