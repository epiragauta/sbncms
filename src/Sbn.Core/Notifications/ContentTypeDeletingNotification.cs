using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class ContentTypeDeletingNotification : DeletingNotification<IContentType>
    {
        public ContentTypeDeletingNotification(IContentType target, EventMessages messages) : base(target, messages)
        {
        }

        public ContentTypeDeletingNotification(IEnumerable<IContentType> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
