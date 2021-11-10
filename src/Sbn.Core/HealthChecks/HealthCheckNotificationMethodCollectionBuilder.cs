using Sbn.Cms.Core.Composing;
using Sbn.Cms.Core.HealthChecks.NotificationMethods;

namespace Sbn.Cms.Core.HealthChecks
{
    public class HealthCheckNotificationMethodCollectionBuilder : LazyCollectionBuilderBase<HealthCheckNotificationMethodCollectionBuilder, HealthCheckNotificationMethodCollection, IHealthCheckNotificationMethod>
    {
        protected override HealthCheckNotificationMethodCollectionBuilder This => this;
    }
}
