using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class EntityRefreshNotification<T> : ObjectNotification<T> where T : class, IContentBase
    {
        public EntityRefreshNotification(T target, EventMessages messages) : base(target, messages)
        {
        }

        public T Entity => Target;
    }
}
