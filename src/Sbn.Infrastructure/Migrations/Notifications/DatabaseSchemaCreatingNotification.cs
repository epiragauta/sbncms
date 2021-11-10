using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Notifications;

namespace Sbn.Cms.Infrastructure.Migrations.Notifications
{
    internal class DatabaseSchemaCreatingNotification : CancelableNotification
    {
        public DatabaseSchemaCreatingNotification(EventMessages messages) : base(messages)
        {
        }
    }
}
