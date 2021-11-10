using System.Runtime.Serialization;
using Sbn.Cms.Core.Sections;

namespace Sbn.Cms.Core.Manifest
{
    [DataContract(Name = "section", Namespace = "")]
    public class ManifestSection : ISection
    {
        [DataMember(Name = "alias")]
        public string Alias { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
