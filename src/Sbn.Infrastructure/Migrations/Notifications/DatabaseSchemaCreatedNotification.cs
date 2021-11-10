using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Notifications;

namespace Sbn.Cms.Infrastructure.Migrations.Notifications
{
    internal class DatabaseSchemaCreatedNotification : StatefulNotification
    {
        public DatabaseSchemaCreatedNotification(EventMessages eventMessages) => EventMessages = eventMessages;

        public EventMessages EventMessages { get; }

    }
}
