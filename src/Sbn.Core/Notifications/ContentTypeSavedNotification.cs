using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class ContentTypeSavedNotification : SavedNotification<IContentType>
    {
        public ContentTypeSavedNotification(IContentType target, EventMessages messages) : base(target, messages)
        {
        }

        public ContentTypeSavedNotification(IEnumerable<IContentType> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
