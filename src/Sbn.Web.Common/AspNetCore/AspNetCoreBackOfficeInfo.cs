using Microsoft.Extensions.Options;
using Sbn.Cms.Core;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.Hosting;
using Sbn.Cms.Core.Routing;
using static Sbn.Cms.Core.Constants;

namespace Sbn.Cms.Web.Common.AspNetCore
{
    public class AspNetCoreBackOfficeInfo : IBackOfficeInfo
    {
        private readonly IOptionsMonitor<GlobalSettings> _globalSettings;
        private readonly IHostingEnvironment _hostingEnvironment;
        private string _getAbsoluteUrl;
        public AspNetCoreBackOfficeInfo(IOptionsMonitor<GlobalSettings> globalSettings, IHostingEnvironment hostingEnviroment)
        {
            _globalSettings = globalSettings;
            _hostingEnvironment = hostingEnviroment;

        }

        public string GetAbsoluteUrl
        {
            get
            {
                if (_getAbsoluteUrl is null)
                {
                    if(_hostingEnvironment.ApplicationMainUrl is null)
                    {
                        return "";
                    }
                    _getAbsoluteUrl = WebPath.Combine(_hostingEnvironment.ApplicationMainUrl.ToString(), _globalSettings.CurrentValue.SbnPath.TrimStart(CharArrays.TildeForwardSlash));
                }
                return _getAbsoluteUrl;
            }
        }
    }
}
