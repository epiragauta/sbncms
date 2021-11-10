using System.Runtime.Serialization;

namespace Sbn.Cms.Core.Dashboards
{
    [DataContract(IsReference = true)]
    public class DashboardSlim : IDashboardSlim
    {
        public string Alias { get; set; }

        public string View { get; set; }
    }
}
