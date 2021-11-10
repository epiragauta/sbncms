using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class EntityContainerRenamedNotification : RenamedNotification<EntityContainer>
    {
        public EntityContainerRenamedNotification(EntityContainer target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
