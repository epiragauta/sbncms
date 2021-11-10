using System;
using System.Collections.Generic;
using Sbn.Cms.Core.Composing;

namespace Sbn.Cms.Core.HealthChecks
{
    public class HealthCheckCollection : BuilderCollectionBase<HealthCheck>
    {
        public HealthCheckCollection(Func<IEnumerable<HealthCheck>> items) : base(items)
        {
        }
    }
}
