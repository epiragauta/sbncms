using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class EntityContainerSavingNotification : SavingNotification<EntityContainer>
    {
        public EntityContainerSavingNotification(EntityContainer target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
