using System;
using Sbn.Cms.Core;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.Hosting;

namespace Sbn.Extensions
{
    public static class GlobalSettingsExtensions
    {
        private static string _mvcArea;
        private static string _backOfficePath;

        /// <summary>
        /// Returns the absolute path for the Sbn back office
        /// </summary>
        /// <param name="globalSettings"></param>
        /// <param name="hostingEnvironment"></param>
        /// <returns></returns>
        public static string GetBackOfficePath(this GlobalSettings globalSettings, IHostingEnvironment hostingEnvironment)
        {
            if (_backOfficePath != null) return _backOfficePath;
            _backOfficePath = hostingEnvironment.ToAbsolute(globalSettings.SbnPath);
            return _backOfficePath;
        }

        /// <summary>
        /// This returns the string of the MVC Area route.
        /// </summary>
        /// <remarks>
        /// This will return the MVC area that we will route all custom routes through like surface controllers, etc...
        /// We will use the 'Path' (default ~/sbn) to create it but since it cannot contain '/' and people may specify a path of ~/asdf/asdf/admin
        /// we will convert the '/' to '-' and use that as the path. its a bit lame but will work.
        ///
        /// We also make sure that the virtual directory (SystemDirectories.Root) is stripped off first, otherwise we'd end up with something
        /// like "MyVirtualDirectory-Sbn" instead of just "Sbn".
        /// </remarks>
        public static string GetSbnMvcArea(this GlobalSettings globalSettings, IHostingEnvironment hostingEnvironment)
        {
            if (_mvcArea != null) return _mvcArea;

            _mvcArea = globalSettings.GetSbnMvcAreaNoCache(hostingEnvironment);

            return _mvcArea;
        }

        internal static string GetSbnMvcAreaNoCache(this GlobalSettings globalSettings, IHostingEnvironment hostingEnvironment)
        {
            var path = string.IsNullOrEmpty(globalSettings.SbnPath)
                ? string.Empty
                : hostingEnvironment.ToAbsolute(globalSettings.SbnPath);

            if (path.IsNullOrWhiteSpace())
                throw new InvalidOperationException("Cannot create an MVC Area path without the sbnPath specified");

            if (path.StartsWith(hostingEnvironment.ApplicationVirtualPath)) // beware of TrimStart, see U4-2518
                path = path.Substring(hostingEnvironment.ApplicationVirtualPath.Length);
            return path.TrimStart(Constants.CharArrays.Tilde).TrimStart(Constants.CharArrays.ForwardSlash).Replace('/', '-').Trim().ToLower();
        }
    }
}
