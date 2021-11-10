using System.Collections.Generic;
using Sbn.Cms.Core.Dashboards;
using Sbn.Cms.Core.Models.ContentEditing;
using Sbn.Cms.Core.Web;

namespace Sbn.Cms.Core.Notifications
{
    public class SendingDashboardsNotification : INotification
    {
        public ISbnContext SbnContext { get; }

        public IEnumerable<Tab<IDashboardSlim>> Dashboards { get; }

        public SendingDashboardsNotification(IEnumerable<Tab<IDashboardSlim>> dashboards, ISbnContext sbnContext)
        {
            Dashboards = dashboards;
            SbnContext = sbnContext;
        }
    }
}
