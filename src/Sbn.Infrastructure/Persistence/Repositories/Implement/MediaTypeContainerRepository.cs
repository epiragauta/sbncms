using Microsoft.Extensions.Logging;
using Sbn.Cms.Core.Cache;
using Sbn.Cms.Core.Persistence.Repositories;
using Sbn.Cms.Core.Scoping;

namespace Sbn.Cms.Infrastructure.Persistence.Repositories.Implement
{
    class MediaTypeContainerRepository : EntityContainerRepository, IMediaTypeContainerRepository
    {
        public MediaTypeContainerRepository(IScopeAccessor scopeAccessor, AppCaches cache, ILogger<MediaTypeContainerRepository> logger)
            : base(scopeAccessor, cache, logger, Cms.Core.Constants.ObjectTypes.MediaTypeContainer)
        { }
    }
}
