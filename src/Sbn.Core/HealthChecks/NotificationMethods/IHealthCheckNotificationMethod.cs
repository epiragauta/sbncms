using System.Threading.Tasks;
using Sbn.Cms.Core.Composing;

namespace Sbn.Cms.Core.HealthChecks.NotificationMethods
{
    public interface IHealthCheckNotificationMethod : IDiscoverable
    {
        bool Enabled { get; }

        Task SendAsync(HealthCheckResults results);
    }
}
