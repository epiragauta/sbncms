using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class EntityContainerDeletingNotification : DeletingNotification<EntityContainer>
    {
        public EntityContainerDeletingNotification(EntityContainer target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
