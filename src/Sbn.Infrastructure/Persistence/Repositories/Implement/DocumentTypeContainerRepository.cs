using Microsoft.Extensions.Logging;
using Sbn.Cms.Core.Cache;
using Sbn.Cms.Core.Persistence.Repositories;
using Sbn.Cms.Core.Scoping;

namespace Sbn.Cms.Infrastructure.Persistence.Repositories.Implement
{
    internal class DocumentTypeContainerRepository : EntityContainerRepository, IDocumentTypeContainerRepository
    {
        public DocumentTypeContainerRepository(IScopeAccessor scopeAccessor, AppCaches cache, ILogger<DataTypeContainerRepository> logger)
            : base(scopeAccessor, cache, logger, Cms.Core.Constants.ObjectTypes.DocumentTypeContainer)
        { }
    }
}
