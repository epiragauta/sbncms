using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class EntityContainerDeletedNotification : DeletedNotification<EntityContainer>
    {
        public EntityContainerDeletedNotification(EntityContainer target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
