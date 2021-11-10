using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Services.Changes;

namespace Sbn.Cms.Core.Notifications
{
    public class ContentTypeChangedNotification : ContentTypeChangeNotification<IContentType>
    {
        public ContentTypeChangedNotification(ContentTypeChange<IContentType> target, EventMessages messages) : base(target, messages)
        {
        }

        public ContentTypeChangedNotification(IEnumerable<ContentTypeChange<IContentType>> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
