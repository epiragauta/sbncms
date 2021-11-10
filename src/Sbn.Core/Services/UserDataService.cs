using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Sbn.Cms.Core.Configuration;
using Sbn.Cms.Core.Models;
using Sbn.Extensions;

namespace Sbn.Cms.Core.Services
{
    public class UserDataService : IUserDataService
    {
        private readonly ISbnVersion _version;
        private readonly ILocalizationService _localizationService;

        public UserDataService(ISbnVersion version, ILocalizationService localizationService)
        {
            _version = version;
            _localizationService = localizationService;
        }

        public IEnumerable<UserData> GetUserData() =>
            new List<UserData>
            {
                new("Server OS", RuntimeInformation.OSDescription),
                new("Server Framework", RuntimeInformation.FrameworkDescription),
                new("Default Language", _localizationService.GetDefaultLanguageIsoCode()),
                new("Sbn Version", _version.SemanticVersion.ToSemanticStringWithoutBuild()),
                new("Current Culture", Thread.CurrentThread.CurrentCulture.ToString()),
                new("Current UI Culture", Thread.CurrentThread.CurrentUICulture.ToString()),
                new("Current Webserver", GetCurrentWebServer())
            };

        private string GetCurrentWebServer() => IsRunningInProcessIIS() ? "IIS" : "Kestrel";

        public bool IsRunningInProcessIIS()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return false;
            }

            string processName = Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().ProcessName);
            return (processName.Contains("w3wp") || processName.Contains("iisexpress"));
        }
    }
}
