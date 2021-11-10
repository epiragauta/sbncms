using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class EntityContainerSavedNotification : SavedNotification<EntityContainer>
    {
        public EntityContainerSavedNotification(EntityContainer target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
