using Sbn.Cms.Core.Models.Entities;
using Sbn.Cms.Core.Semver;

namespace Sbn.Cms.Core.Models
{
    public interface IMigrationEntry : IEntity, IRememberBeingDirty
    {
        string MigrationName { get; set; }
        SemVersion Version { get; set; }
    }
}
