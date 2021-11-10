using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Options;
using Sbn.Cms.Core.Configuration;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.Hosting;
using Sbn.Extensions;

namespace Sbn.Cms.Core.Routing
{
    /// <summary>
    /// Utility for checking paths
    /// </summary>
    public class SbnRequestPaths
    {
        private readonly string _backOfficePath;
        private readonly string _mvcArea;
        private readonly string _backOfficeMvcPath;
        private readonly string _previewMvcPath;
        private readonly string _surfaceMvcPath;
        private readonly string _apiMvcPath;
        private readonly string _installPath;
        private readonly string _appPath;
        private readonly List<string> _defaultUmbPaths;

        /// <summary>
        /// Initializes a new instance of the <see cref="SbnRequestPaths"/> class.
        /// </summary>
        public SbnRequestPaths(IOptions<GlobalSettings> globalSettings, IHostingEnvironment hostingEnvironment)
        {
            var applicationPath = hostingEnvironment.ApplicationVirtualPath;
            _appPath = applicationPath.TrimStart(Constants.CharArrays.ForwardSlash);

            _backOfficePath = globalSettings.Value.GetBackOfficePath(hostingEnvironment)
                .EnsureStartsWith('/').TrimStart(_appPath.EnsureStartsWith('/')).EnsureStartsWith('/');

            _mvcArea = globalSettings.Value.GetSbnMvcArea(hostingEnvironment);
            _defaultUmbPaths = new List<string> { "/" + _mvcArea, "/" + _mvcArea + "/" };
            _backOfficeMvcPath = "/" + _mvcArea + "/BackOffice/";
            _previewMvcPath = "/" + _mvcArea + "/Preview/";
            _surfaceMvcPath = "/" + _mvcArea + "/Surface/";
            _apiMvcPath = "/" + _mvcArea + "/Api/";
            _installPath = hostingEnvironment.ToAbsolute(Constants.SystemDirectories.Install);
        }

        /// <summary>
        /// Checks if the current uri is a back office request
        /// </summary>
        /// <remarks>
        /// <para>
        /// There are some special routes we need to check to properly determine this:
        /// </para>
        /// <para>
        ///     These are def back office:
        ///         /Sbn/BackOffice     = back office
        ///         /Sbn/Preview        = back office
        /// </para>
        /// <para>
        ///     If it's not any of the above then we cannot determine if it's back office or front-end
        ///     so we can only assume that it is not back office. This will occur if people use an SbnApiController for the backoffice
        ///     but do not inherit from SbnAuthorizedApiController and do not use [IsBackOffice] attribute.
        /// </para>
        /// <para>
        ///     These are def front-end:
        ///         /Sbn/Surface        = front-end
        ///         /Sbn/Api            = front-end
        ///     But if we've got this far we'll just have to assume it's front-end anyways.
        /// </para>
        /// </remarks>
        public bool IsBackOfficeRequest(string absPath)
        {
            var fullUrlPath = absPath.TrimStart(Constants.CharArrays.ForwardSlash);
            var urlPath = fullUrlPath.TrimStart(_appPath).EnsureStartsWith('/');

            // check if this is in the sbn back office
            var isSbnPath = urlPath.InvariantStartsWith(_backOfficePath);

            // if not, then def not back office
            if (isSbnPath == false)
            {
                return false;
            }

            // if its the normal /sbn path
            if (_defaultUmbPaths.Any(x => urlPath.InvariantEquals(x)))
            {
                return true;
            }

            // check for special back office paths
            if (urlPath.InvariantStartsWith(_backOfficeMvcPath)
                || urlPath.InvariantStartsWith(_previewMvcPath))
            {
                return true;
            }

            // check for special front-end paths
            if (urlPath.InvariantStartsWith(_surfaceMvcPath)
                || urlPath.InvariantStartsWith(_apiMvcPath))
            {
                return false;
            }

            // if its none of the above, we will have to try to detect if it's a PluginController route, we can detect this by
            // checking how many parts the route has, for example, all PluginController routes will be routed like
            // Sbn/MYPLUGINAREA/MYCONTROLLERNAME/{action}/{id}
            // so if the path contains at a minimum 3 parts: Sbn + MYPLUGINAREA + MYCONTROLLERNAME then we will have to assume it is a
            // plugin controller for the front-end.
            if (urlPath.Split(Constants.CharArrays.ForwardSlash, StringSplitOptions.RemoveEmptyEntries).Length >= 3)
            {
                return false;
            }

            // if its anything else we can assume it's back office
            return true;
        }

        /// <summary>
        /// Checks if the current uri is an install request
        /// </summary>
        public bool IsInstallerRequest(string absPath) => absPath.InvariantStartsWith(_installPath);

        /// <summary>
        /// Rudimentary check to see if it's not a server side request
        /// </summary>
        public bool IsClientSideRequest(string absPath)
        {
            var ext = Path.GetExtension(absPath);
            return !ext.IsNullOrWhiteSpace();
        }
    }
}
