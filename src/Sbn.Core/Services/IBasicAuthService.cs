using System.Net;

namespace Sbn.Cms.Core.Services
{
    public interface IBasicAuthService
    {
        bool IsBasicAuthEnabled();
        bool IsIpAllowListed(IPAddress clientIpAddress);
    }
}
