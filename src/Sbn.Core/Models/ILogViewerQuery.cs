using Sbn.Cms.Core.Models.Entities;

namespace Sbn.Cms.Core.Models
{
    public interface ILogViewerQuery : IEntity
    {
        string Name { get; set; }
        string Query { get; set; }
    }
}
