using System.Collections.Generic;
using System.Runtime.Serialization;
using Sbn.Cms.Core.Models.Editors;

namespace Sbn.Cms.Core.Models.ContentEditing
{
    /// <summary>
    /// This is used for the response of PostAddFile so that we can analyze the response in a filter and remove the
    /// temporary files that were created.
    /// </summary>
    [DataContract]
    public class PostedFiles : IHaveUploadedFiles, INotificationModel
    {
        public PostedFiles()
        {
            UploadedFiles = new List<ContentPropertyFile>();
            Notifications = new List<BackOfficeNotification>();
        }
        public List<ContentPropertyFile> UploadedFiles { get; private set; }

        [DataMember(Name = "notifications")]
        public List<BackOfficeNotification> Notifications { get; private set; }
    }
}
