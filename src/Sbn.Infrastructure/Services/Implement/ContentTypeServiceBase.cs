using Microsoft.Extensions.Logging;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Scoping;

namespace Sbn.Cms.Core.Services.Implement
{
    public abstract class ContentTypeServiceBase : RepositoryService
    {
        protected ContentTypeServiceBase(IScopeProvider provider, ILoggerFactory loggerFactory, IEventMessagesFactory eventMessagesFactory)
            : base(provider, loggerFactory, eventMessagesFactory)
        { }
    }
}
