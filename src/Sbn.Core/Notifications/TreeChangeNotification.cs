using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Services.Changes;

namespace Sbn.Cms.Core.Notifications
{
    public abstract class TreeChangeNotification<T> : EnumerableObjectNotification<TreeChange<T>>
    {
        protected TreeChangeNotification(TreeChange<T> target, EventMessages messages) : base(target, messages)
        {
        }

        protected TreeChangeNotification(IEnumerable<TreeChange<T>> target, EventMessages messages) : base(target, messages)
        {
        }

        public IEnumerable<TreeChange<T>> Changes => Target;
    }
}
