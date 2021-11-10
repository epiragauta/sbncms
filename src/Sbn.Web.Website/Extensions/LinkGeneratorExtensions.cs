using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Routing;
using Sbn.Cms.Core;
using Sbn.Cms.Web.Website.Controllers;

namespace Sbn.Extensions
{
    public static class LinkGeneratorExtensions
    {
        /// <summary>
        /// Return the Url for a Surface Controller
        /// </summary>
        /// <typeparam name="T">The <see cref="SurfaceController"/></typeparam>
        public static string GetSbnSurfaceUrl<T>(this LinkGenerator linkGenerator, Expression<Func<T, object>> methodSelector)
            where T : SurfaceController
        {
            MethodInfo method = ExpressionHelper.GetMethodInfo(methodSelector);
            IDictionary<string, object> methodParams = ExpressionHelper.GetMethodParams(methodSelector);

            if (method == null)
            {
                throw new MissingMethodException(
                    $"Could not find the method {methodSelector} on type {typeof(T)} or the result ");
            }

            if (methodParams.Any() == false)
            {
                return linkGenerator.GetSbnSurfaceUrl<T>(method.Name);
            }

            return linkGenerator.GetSbnSurfaceUrl<T>(method.Name, methodParams);
        }

        /// <summary>
        /// Return the Url for a Surface Controller
        /// </summary>
        /// <typeparam name="T">The <see cref="SurfaceController"/></typeparam>
        public static string GetSbnSurfaceUrl<T>(this LinkGenerator linkGenerator, string actionName, object id = null)
            where T : SurfaceController => linkGenerator.GetSbnControllerUrl(
                actionName,
                typeof(T),
                new Dictionary<string, object>()
                {
                    ["id"] = id
                });
    }
}
