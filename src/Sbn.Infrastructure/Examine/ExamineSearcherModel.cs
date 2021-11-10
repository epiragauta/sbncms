using System.Runtime.Serialization;

namespace Sbn.Cms.Infrastructure.Examine
{
    [DataContract(Name = "searcher", Namespace = "")]
    public class ExamineSearcherModel
    {
        public ExamineSearcherModel()
        {
        }

        [DataMember(Name = "name")]
        public string Name { get; set; }

    }

}
