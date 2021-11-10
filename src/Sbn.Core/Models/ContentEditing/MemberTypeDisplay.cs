using System.Runtime.Serialization;

namespace Sbn.Cms.Core.Models.ContentEditing
{
    [DataContract(Name = "contentType", Namespace = "")]
    public class MemberTypeDisplay : ContentTypeCompositionDisplay<MemberPropertyTypeDisplay>
    {
    }
}
