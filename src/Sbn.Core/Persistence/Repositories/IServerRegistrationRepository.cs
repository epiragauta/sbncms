using System;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Persistence.Repositories
{
    public interface IServerRegistrationRepository : IReadWriteQueryRepository<int, IServerRegistration>
    {
        void DeactiveStaleServers(TimeSpan staleTimeout);
    }
}
