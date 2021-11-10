using System;
using System.Collections.Generic;
using Sbn.Cms.Core.Composing;

namespace Sbn.Cms.Core.Dashboards
{
    public class DashboardCollection : BuilderCollectionBase<IDashboard>
    {
        public DashboardCollection(Func<IEnumerable<IDashboard>> items) : base(items)
        {
        }
    }
}
