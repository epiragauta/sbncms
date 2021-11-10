using Sbn.Cms.Core.Models.Entities;

namespace Sbn.Cms.Core.Models
{
    public interface IStylesheetProperty : IRememberBeingDirty
    {
        string Alias { get; set; }
        string Name { get;  }
        string Value { get; set; }
    }
}
