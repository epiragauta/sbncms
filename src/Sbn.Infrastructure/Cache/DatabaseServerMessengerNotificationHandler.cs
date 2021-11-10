// Copyright (c) Sbn.
// See LICENSE for more details.

using Microsoft.Extensions.Logging;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Notifications;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Sync;
using Sbn.Cms.Infrastructure.Persistence;

namespace Sbn.Cms.Core.Cache
{
    /// <summary>
    /// Ensures that distributed cache events are setup and the <see cref="IServerMessenger"/> is initialized
    /// </summary>
    public sealed class DatabaseServerMessengerNotificationHandler : INotificationHandler<SbnApplicationStartingNotification>, INotificationHandler<SbnRequestEndNotification>
    {
        private readonly IServerMessenger _messenger;
        private readonly ISbnDatabaseFactory _databaseFactory;
        private readonly ILogger<DatabaseServerMessengerNotificationHandler> _logger;
        private readonly IRuntimeState _runtimeState;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseServerMessengerNotificationHandler"/> class.
        /// </summary>
        public DatabaseServerMessengerNotificationHandler(
            IServerMessenger serverMessenger,
            ISbnDatabaseFactory databaseFactory,
            ILogger<DatabaseServerMessengerNotificationHandler> logger,
            IRuntimeState runtimeState)
        {
            _databaseFactory = databaseFactory;
            _logger = logger;
            _messenger = serverMessenger;
            _runtimeState = runtimeState;
        }

        /// <inheritdoc/>
        public void Handle(SbnApplicationStartingNotification notification)
        {
            if (_runtimeState.Level != RuntimeLevel.Run)
            {
                return;
            }

            if (_databaseFactory.CanConnect == false)
			{
				_logger.LogWarning("Cannot connect to the database, distributed calls will not be enabled for this server.");
                return;
            }

            // Sync on startup, this will run through the messenger's initialization sequence
            _messenger?.Sync();
        }

        /// <summary>
        /// Clear the batch on end request
        /// </summary>
        public void Handle(SbnRequestEndNotification notification) => _messenger?.SendMessages();
    }
}
