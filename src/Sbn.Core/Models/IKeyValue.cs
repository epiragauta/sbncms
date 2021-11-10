using Sbn.Cms.Core.Models.Entities;

namespace Sbn.Cms.Core.Models
{
    public interface IKeyValue : IEntity
    {
        string Identifier { get; set; }

        string Value { get; set; }
    }
}
