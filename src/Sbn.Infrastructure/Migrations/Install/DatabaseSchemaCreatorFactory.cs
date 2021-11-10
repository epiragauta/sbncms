using Microsoft.Extensions.Logging;
using Sbn.Cms.Core.Configuration;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Infrastructure.Persistence;

namespace Sbn.Cms.Infrastructure.Migrations.Install
{
    /// <summary>
    /// Creates the initial database schema during install.
    /// </summary>
    public class DatabaseSchemaCreatorFactory
    {
        private readonly ILogger<DatabaseSchemaCreator> _logger;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ISbnVersion _sbnVersion;
        private readonly IEventAggregator _eventAggregator;

        public DatabaseSchemaCreatorFactory(
            ILogger<DatabaseSchemaCreator> logger,
            ILoggerFactory loggerFactory,
            ISbnVersion sbnVersion,
            IEventAggregator eventAggregator)
        {
            _logger = logger;
            _loggerFactory = loggerFactory;
            _sbnVersion = sbnVersion;
            _eventAggregator = eventAggregator;
        }

        public DatabaseSchemaCreator Create(ISbnDatabase database)
        {
            return new DatabaseSchemaCreator(database, _logger, _loggerFactory, _sbnVersion, _eventAggregator);
        }
    }
}
