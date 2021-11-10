using System;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Persistence.Repositories
{
    public interface IRelationTypeRepository : IReadWriteQueryRepository<int, IRelationType>, IReadRepository<Guid, IRelationType>
    { }
}
