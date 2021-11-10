using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Web.BackOffice.Filters;
using Sbn.Cms.Web.Common.Attributes;
using Sbn.Cms.Web.Common.Controllers;

namespace Sbn.Cms.Web.BackOffice.Controllers
{
    [PluginController("SbnApi")]
    [IsBackOffice]
    [SbnRequireHttps]
    [MiddlewareFilter(typeof(UnhandledExceptionLoggerFilter))]
    public class IconController : SbnApiController
    {
        private readonly IIconService _iconService;

        public IconController(IIconService iconService)
        {
            _iconService = iconService;
        }

        /// <summary>
        /// Gets an IconModel containing the icon name and SvgString according to an icon name found at the global icons path
        /// </summary>
        /// <param name="iconName"></param>
        /// <returns></returns>
        public IconModel GetIcon(string iconName)
        {
            return _iconService.GetIcon(iconName);
        }


        /// <summary>
        /// Gets a list of all svg icons found at at the global icons path.
        /// </summary>
        /// <returns></returns>
        public IReadOnlyDictionary<string, string> GetIcons() => _iconService.GetIcons();
    }


}
