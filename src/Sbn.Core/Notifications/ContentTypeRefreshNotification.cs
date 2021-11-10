using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Services.Changes;

namespace Sbn.Cms.Core.Notifications
{
    public abstract class ContentTypeRefreshNotification<T> : ContentTypeChangeNotification<T> where T: class, IContentTypeComposition
    {
        protected ContentTypeRefreshNotification(ContentTypeChange<T> target, EventMessages messages) : base(target, messages)
        {
        }

        protected ContentTypeRefreshNotification(IEnumerable<ContentTypeChange<T>> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
