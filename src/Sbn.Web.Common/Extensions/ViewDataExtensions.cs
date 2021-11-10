using System;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Sbn.Cms.Core.Security;
using Sbn.Cms.Core.Semver;
using Sbn.Cms.Core.Serialization;

namespace Sbn.Extensions
{
    public static class ViewDataExtensions
    {
        public const string TokenSbnPath = "SbnPath";
        public const string TokenInstallApiBaseUrl = "InstallApiBaseUrl";
        public const string TokenSbnBaseFolder = "SbnBaseFolder";
        public const string TokenSbnVersion = "SbnVersion";
        public const string TokenExternalSignInError = "ExternalSignInError";
        public const string TokenPasswordResetCode = "PasswordResetCode";

        public static bool FromTempData(this ViewDataDictionary viewData, ITempDataDictionary tempData, string token)
        {
            if (tempData[token] == null) return false;
            viewData[token] = tempData[token];
            return true;
        }

        /// <summary>
        /// Copies data from a request cookie to view data and then clears the cookie in the response
        /// </summary>
        /// <param name="viewData"></param>
        /// <param name="httpContext"></param>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        /// <remarks>
        /// <para>
        /// This is similar to TempData but in some cases we cannot use TempData which relies on the temp data provider and session.
        /// The cookie value can either be a simple string value
        /// </para>
        /// </remarks>
        public static bool FromBase64CookieData<T>(this ViewDataDictionary viewData, HttpContext httpContext, string cookieName, IJsonSerializer serializer)
        {
            var hasCookie = httpContext.Request.Cookies.ContainsKey(cookieName);
            if (!hasCookie) return false;

            // get the cookie value
            if (!httpContext.Request.Cookies.TryGetValue(cookieName, out var cookieVal))
            {
                return false;
            }

            // ensure the cookie is expired (must be done after reading the value)
            httpContext.Response.Cookies.Delete(cookieName);

            if (cookieVal.IsNullOrWhiteSpace())
                return false;

            try
            {
                var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(System.Net.WebUtility.UrlDecode(cookieVal)));
                // deserialize to T and store in viewdata
                viewData[cookieName] = serializer.Deserialize<T>(decoded);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string GetSbnPath(this ViewDataDictionary viewData)
        {
            return (string)viewData[TokenSbnPath];
        }

        public static void SetSbnPath(this ViewDataDictionary viewData, string value)
        {
            viewData[TokenSbnPath] = value;
        }

        public static string GetInstallApiBaseUrl(this ViewDataDictionary viewData)
        {
            return (string)viewData[TokenInstallApiBaseUrl];
        }

        public static void SetInstallApiBaseUrl(this ViewDataDictionary viewData, string value)
        {
            viewData[TokenInstallApiBaseUrl] = value;
        }

        public static string GetSbnBaseFolder(this ViewDataDictionary viewData)
        {
            return (string)viewData[TokenSbnBaseFolder];
        }

        public static void SetSbnBaseFolder(this ViewDataDictionary viewData, string value)
        {
            viewData[TokenSbnBaseFolder] = value;
        }
        public static void SetSbnVersion(this ViewDataDictionary viewData, SemVersion version)
        {
            viewData[TokenSbnVersion] = version;
        }

        public static SemVersion GetSbnVersion(this ViewDataDictionary viewData)
        {
            return (SemVersion) viewData[TokenSbnVersion];
        }

        /// <summary>
        /// Used by the back office login screen to get any registered external login provider errors
        /// </summary>
        /// <param name="viewData"></param>
        /// <returns></returns>
        public static BackOfficeExternalLoginProviderErrors GetExternalSignInProviderErrors(this ViewDataDictionary viewData)
        {
            return (BackOfficeExternalLoginProviderErrors)viewData[TokenExternalSignInError];
        }

        /// <summary>
        /// Used by the back office controller to register any external login provider errors
        /// </summary>
        /// <param name="viewData"></param>
        /// <param name="errors"></param>
        public static void SetExternalSignInProviderErrors(this ViewDataDictionary viewData, BackOfficeExternalLoginProviderErrors errors)
        {
            viewData[TokenExternalSignInError] = errors;
        }

        public static string GetPasswordResetCode(this ViewDataDictionary viewData)
        {
            return (string)viewData[TokenPasswordResetCode];
        }

        public static void SetPasswordResetCode(this ViewDataDictionary viewData, string value)
        {
            viewData[TokenPasswordResetCode] = value;
        }
    }
}
