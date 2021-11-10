using System;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Persistence.Repositories
{
    public interface IPublicAccessRepository : IReadWriteQueryRepository<Guid, PublicAccessEntry>
    { }
}
