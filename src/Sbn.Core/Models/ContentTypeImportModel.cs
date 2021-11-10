using System.Collections.Generic;
using System.Runtime.Serialization;
using Sbn.Cms.Core.Models.ContentEditing;

namespace Sbn.Cms.Core.Models
{
    [DataContract(Name = "contentTypeImportModel")]
    public class ContentTypeImportModel : INotificationModel
    {
        [DataMember(Name = "alias")]
        public string Alias { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "notifications")]
        public List<BackOfficeNotification> Notifications { get; } = new List<BackOfficeNotification>();

        [DataMember(Name = "tempFileName")]
        public string TempFileName { get; set; }
    }
}
