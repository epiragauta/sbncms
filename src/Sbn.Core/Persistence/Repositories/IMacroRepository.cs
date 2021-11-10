using System;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Persistence.Repositories
{
    public interface IMacroRepository : IReadWriteQueryRepository<int, IMacro>, IReadRepository<Guid, IMacro>
    {

        //IEnumerable<IMacro> GetAll(params string[] aliases);

    }
}
