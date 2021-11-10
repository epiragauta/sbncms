using System;
using System.Collections.Generic;
using Sbn.Cms.Core.Composing;
using Sbn.Cms.Core.HealthChecks.NotificationMethods;

namespace Sbn.Cms.Core.HealthChecks
{
    public class HealthCheckNotificationMethodCollection : BuilderCollectionBase<IHealthCheckNotificationMethod>
    {
        public HealthCheckNotificationMethodCollection(Func<IEnumerable<IHealthCheckNotificationMethod>> items) : base(items)
        {
        }
    }
}
