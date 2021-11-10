using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class EntityContainerRenamingNotification : RenamingNotification<EntityContainer>
    {
        public EntityContainerRenamingNotification(EntityContainer target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
