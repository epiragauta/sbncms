using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Services.Changes;

namespace Sbn.Cms.Core.Notifications
{
    public abstract class ContentTypeChangeNotification<T> : EnumerableObjectNotification<ContentTypeChange<T>> where T : class, IContentTypeComposition
    {
        protected ContentTypeChangeNotification(ContentTypeChange<T> target, EventMessages messages) : base(target, messages)
        {
        }

        protected ContentTypeChangeNotification(IEnumerable<ContentTypeChange<T>> target, EventMessages messages) : base(target, messages)
        {
        }

        public IEnumerable<ContentTypeChange<T>> Changes => Target;
    }
}
