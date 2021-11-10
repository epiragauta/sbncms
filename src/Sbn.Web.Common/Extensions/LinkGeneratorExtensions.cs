using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Sbn.Cms.Core;
using Sbn.Cms.Core.Hosting;
using Sbn.Cms.Core.Web.Mvc;
using Sbn.Cms.Web.Common.Controllers;

namespace Sbn.Extensions
{
    public static class LinkGeneratorExtensions
    {
        /// <summary>
        /// Return the back office url if the back office is installed
        /// </summary>
        public static string GetBackOfficeUrl(this LinkGenerator linkGenerator, IHostingEnvironment hostingEnvironment)
        {

            Type backOfficeControllerType;
            try
            {
                backOfficeControllerType = Assembly.Load("Sbn.Web.BackOffice")?.GetType("Sbn.Web.BackOffice.Controllers.BackOfficeController");
                if (backOfficeControllerType == null)
                {
                    return "/"; // this would indicate that the installer is installed without the back office
                }
            }
            catch
            {
                return hostingEnvironment.ApplicationVirtualPath; // this would indicate that the installer is installed without the back office
            }

            return linkGenerator.GetPathByAction("Default", ControllerExtensions.GetControllerName(backOfficeControllerType), values: new { area = Cms.Core.Constants.Web.Mvc.BackOfficeApiArea });
        }

        /// <summary>
        /// Return the Url for a Web Api service
        /// </summary>
        /// <typeparam name="T">The <see cref="SbnApiControllerBase"/></typeparam>
        public static string GetSbnApiService<T>(this LinkGenerator linkGenerator, string actionName, object id = null)
            where T : SbnApiControllerBase => linkGenerator.GetSbnControllerUrl(
                actionName,
                typeof(T),
                new Dictionary<string, object>()
                {
                    ["id"] = id
                });

        public static string GetSbnApiService<T>(this LinkGenerator linkGenerator, string actionName, IDictionary<string, object> values)
            where T : SbnApiControllerBase => linkGenerator.GetSbnControllerUrl(actionName, typeof(T), values);

        public static string GetSbnApiServiceBaseUrl<T>(this LinkGenerator linkGenerator, Expression<Func<T, object>> methodSelector)
            where T : SbnApiControllerBase
        {
            var method = ExpressionHelper.GetMethodInfo(methodSelector);
            if (method == null)
            {
                throw new MissingMethodException("Could not find the method " + methodSelector + " on type " + typeof(T) + " or the result ");
            }
            return linkGenerator.GetSbnApiService<T>(method.Name).TrimEnd(method.Name);
        }

        /// <summary>
        /// Return the Url for an Sbn controller
        /// </summary>
        public static string GetSbnControllerUrl(this LinkGenerator linkGenerator, string actionName, string controllerName, string area, IDictionary<string, object> dict = null)
        {
            if (actionName == null)
            {
                throw new ArgumentNullException(nameof(actionName));
            }

            if (string.IsNullOrWhiteSpace(actionName))
            {
                throw new ArgumentException("Value can't be empty or consist only of white-space characters.", nameof(actionName));
            }

            if (controllerName == null)
            {
                throw new ArgumentNullException(nameof(controllerName));
            }

            if (string.IsNullOrWhiteSpace(controllerName))
            {
                throw new ArgumentException("Value can't be empty or consist only of white-space characters.", nameof(controllerName));
            }

            if (dict is null)
            {
                dict = new Dictionary<string, object>();
            }

            if (!area.IsNullOrWhiteSpace())
            {
                dict["area"] = area;
            }

            IDictionary<string, object> values = dict.Aggregate(
                new ExpandoObject() as IDictionary<string, object>,
                (a, p) =>
                {
                    a.Add(p.Key, p.Value);
                    return a;
                });

            return linkGenerator.GetPathByAction(actionName, controllerName, values);
        }

        /// <summary>
        /// Return the Url for an Sbn controller
        /// </summary>
        public static string GetSbnControllerUrl(this LinkGenerator linkGenerator, string actionName, Type controllerType, IDictionary<string, object> values = null)
        {
            if (actionName == null)
            {
                throw new ArgumentNullException(nameof(actionName));
            }

            if (string.IsNullOrWhiteSpace(actionName))
            {
                throw new ArgumentException("Value can't be empty or consist only of white-space characters.", nameof(actionName));
            }

            if (controllerType == null)
            {
                throw new ArgumentNullException(nameof(controllerType));
            }

            var area = string.Empty;

            if (!typeof(ControllerBase).IsAssignableFrom(controllerType))
            {
                throw new InvalidOperationException($"The controller {controllerType} is of type {typeof(ControllerBase)}");
            }

            PluginControllerMetadata metaData = PluginController.GetMetadata(controllerType);
            if (metaData.AreaName.IsNullOrWhiteSpace() == false)
            {
                // set the area to the plugin area
                area = metaData.AreaName;
            }

            return linkGenerator.GetSbnControllerUrl(actionName, ControllerExtensions.GetControllerName(controllerType), area, values);
        }

        public static string GetSbnApiService<T>(this LinkGenerator linkGenerator, Expression<Func<T, object>> methodSelector)
            where T : SbnApiController
        {
            var method = ExpressionHelper.GetMethodInfo(methodSelector);
            var methodParams = ExpressionHelper.GetMethodParams(methodSelector);
            if (method == null)
            {
                throw new MissingMethodException(
                    $"Could not find the method {methodSelector} on type {typeof(T)} or the result ");
            }

            if (methodParams.Any() == false)
            {
                return linkGenerator.GetSbnApiService<T>(method.Name);
            }

            return linkGenerator.GetSbnApiService<T>(method.Name, methodParams);
        }
    }
}
