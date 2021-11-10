using System.Runtime.Serialization;

namespace Sbn.Cms.Core.Models.ContentEditing
{
    [DataContract(Name = "contentType", Namespace = "")]
    public class MediaTypeDisplay : ContentTypeCompositionDisplay<PropertyTypeDisplay>
    {
        [DataMember(Name = "isSystemMediaType")]
        public bool IsSystemMediaType { get; set; }
    }
}
