using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Sbn.Cms.Core.Models
{

    [DataContract(Name = "requestPasswordReset", Namespace = "")]
    public class RequestPasswordResetModel
    {
        [Required]
        [DataMember(Name = "email", IsRequired = true)]
        public string Email { get; set; }
    }
}
