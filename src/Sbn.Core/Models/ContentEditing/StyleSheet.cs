using System.Runtime.Serialization;

namespace Sbn.Cms.Core.Models.ContentEditing
{
    [DataContract(Name = "stylesheet", Namespace = "")]
    public class Stylesheet
    {
        [DataMember(Name="name")]
        public string Name { get; set; }

        [DataMember(Name = "path")]
        public string Path { get; set; }
    }
}
