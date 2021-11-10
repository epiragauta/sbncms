using System.Net;
using System.Runtime.Serialization;
using Sbn.Cms.Core.Configuration;

namespace Sbn.Cms.Core.Models
{
    [DataContract(Name = "upgrade", Namespace = "")]
    public class UpgradeCheckResponse
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "comment")]
        public string Comment { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }

        public UpgradeCheckResponse() { }
        public UpgradeCheckResponse(string upgradeType, string upgradeComment, string upgradeUrl, ISbnVersion sbnVersion)
        {
            Type = upgradeType;
            Comment = upgradeComment;
            Url = upgradeUrl + "?version=" + WebUtility.UrlEncode(sbnVersion.Version.ToString(3));
        }
    }
}
