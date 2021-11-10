using System;
using Microsoft.Extensions.Logging;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Persistence.Querying;
using Sbn.Cms.Core.Scoping;

namespace Sbn.Cms.Core.Services.Implement
{
    /// <summary>
    /// Represents a service that works on top of repositories.
    /// </summary>
    public abstract class RepositoryService : IService
    {
        protected IEventMessagesFactory EventMessagesFactory { get; }
        protected IScopeProvider ScopeProvider { get; }
        protected ILoggerFactory LoggerFactory { get; }

        protected RepositoryService(IScopeProvider provider, ILoggerFactory loggerFactory, IEventMessagesFactory eventMessagesFactory)
        {
            EventMessagesFactory = eventMessagesFactory ?? throw new ArgumentNullException(nameof(eventMessagesFactory));
            ScopeProvider = provider ?? throw new ArgumentNullException(nameof(provider));
            LoggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        protected IQuery<T> Query<T>() => ScopeProvider.SqlContext.Query<T>();
    }
}
