using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sbn.Cms.Core;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Exceptions;
using Sbn.Cms.Core.Notifications;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Infrastructure.Migrations.Install;
using Sbn.Cms.Infrastructure.Persistence;

namespace Sbn.Cms.Infrastructure.Install
{
    public class UnattendedInstaller : INotificationAsyncHandler<RuntimeUnattendedInstallNotification>
    {
        private readonly IOptions<UnattendedSettings> _unattendedSettings;

        private readonly DatabaseSchemaCreatorFactory _databaseSchemaCreatorFactory;
        private readonly IEventAggregator _eventAggregator;
        private readonly ISbnDatabaseFactory _databaseFactory;
        private readonly IDbProviderFactoryCreator _dbProviderFactoryCreator;
        private readonly ILogger<UnattendedInstaller> _logger;
        private readonly IRuntimeState _runtimeState;

        public UnattendedInstaller(
            DatabaseSchemaCreatorFactory databaseSchemaCreatorFactory,
            IEventAggregator eventAggregator,
            IOptions<UnattendedSettings> unattendedSettings,
            ISbnDatabaseFactory databaseFactory,
            IDbProviderFactoryCreator dbProviderFactoryCreator,
            ILogger<UnattendedInstaller> logger,
            IRuntimeState runtimeState)
        {
            _databaseSchemaCreatorFactory = databaseSchemaCreatorFactory ?? throw new ArgumentNullException(nameof(databaseSchemaCreatorFactory));
            _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
            _unattendedSettings = unattendedSettings;
            _databaseFactory = databaseFactory;
            _dbProviderFactoryCreator = dbProviderFactoryCreator;
            _logger = logger;
            _runtimeState = runtimeState;
        }

        public Task HandleAsync(RuntimeUnattendedInstallNotification notification, CancellationToken cancellationToken)
        {
            // unattended install is not enabled
            if (_unattendedSettings.Value.InstallUnattended == false)
            {
                return Task.CompletedTask;
            }

            // no connection string set
            if (_databaseFactory.Configured == false)
            {
                return Task.CompletedTask;
            }

            _runtimeState.DetermineRuntimeLevel();
            if (_runtimeState.Reason == RuntimeLevelReason.InstallMissingDatabase)
            {
                _dbProviderFactoryCreator.CreateDatabase(_databaseFactory.ProviderName, _databaseFactory.ConnectionString);
            }

            bool connect;
            try
            {
                for (var i = 0; ;)
                {
                    connect = _databaseFactory.CanConnect;
                    if (connect || ++i == 5)
                    {
                        break;
                    }

                    _logger.LogDebug("Could not immediately connect to database, trying again.");

                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Error during unattended install.");

                var innerException = new UnattendedInstallException("Unattended installation failed.", ex);
                _runtimeState.Configure(Core.RuntimeLevel.BootFailed, Core.RuntimeLevelReason.BootFailedOnException, innerException);
                return Task.CompletedTask;
            }

            // could not connect to the database
            if (connect == false)
            {
                return Task.CompletedTask;
            }

            ISbnDatabase database = null;
            try
            {
                using (database = _databaseFactory.CreateDatabase())
                {
                    var hasSbnTables = database.IsSbnInstalled();

                    // database has sbn tables, assume Sbn is already installed
                    if (hasSbnTables)
                    {
                        return Task.CompletedTask;
                    }

                    // all conditions fulfilled, do the install
                    _logger.LogInformation("Starting unattended install.");

                    database.BeginTransaction();
                    DatabaseSchemaCreator creator = _databaseSchemaCreatorFactory.Create(database);
                    creator.InitializeDatabaseSchema();
                    database.CompleteTransaction();
                    _logger.LogInformation("Unattended install completed.");

                    // Emit an event with EventAggregator that unattended install completed
                    // Then this event can be listened for and create an unattended user
                    _eventAggregator.Publish(new UnattendedInstallNotification());
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Error during unattended install.");
                database?.AbortTransaction();

                var innerException = new UnattendedInstallException(
                    "The database configuration failed."
                    + "\n Please check log file for additional information (can be found in '/Sbn/Data/Logs/')",
                    ex);

                _runtimeState.Configure(Core.RuntimeLevel.BootFailed, Core.RuntimeLevelReason.BootFailedOnException, innerException);
            }

            return Task.CompletedTask;
        }
    }
}
