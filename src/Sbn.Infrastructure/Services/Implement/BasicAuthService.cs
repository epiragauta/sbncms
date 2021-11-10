using System.Net;
using Microsoft.Extensions.Options;
using Sbn.Cms.Core.Configuration.Models;

namespace Sbn.Cms.Core.Services.Implement
{
    public class BasicAuthService : IBasicAuthService
    {
        private BasicAuthSettings _basicAuthSettings;

        public BasicAuthService(IOptionsMonitor<BasicAuthSettings> optionsMonitor)
        {
            _basicAuthSettings = optionsMonitor.CurrentValue;

            optionsMonitor.OnChange(basicAuthSettings => _basicAuthSettings = basicAuthSettings);
        }

        public bool IsBasicAuthEnabled() => _basicAuthSettings.Enabled;

        public bool IsIpAllowListed(IPAddress clientIpAddress)
        {
            foreach (var allowedIpString in _basicAuthSettings.AllowedIPs)
            {
                if (IPNetwork.TryParse(allowedIpString, out IPNetwork allowedIp) && allowedIp.Contains(clientIpAddress))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
