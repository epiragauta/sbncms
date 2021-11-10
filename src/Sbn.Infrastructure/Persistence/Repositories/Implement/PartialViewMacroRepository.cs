using Sbn.Cms.Core.IO;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Persistence.Repositories;

namespace Sbn.Cms.Infrastructure.Persistence.Repositories.Implement
{
    internal class PartialViewMacroRepository : PartialViewRepository, IPartialViewMacroRepository
    {
        public PartialViewMacroRepository(FileSystems fileSystems)
            : base(fileSystems.MacroPartialsFileSystem)
        { }

        protected override PartialViewType ViewType => PartialViewType.PartialViewMacro;
    }
}
