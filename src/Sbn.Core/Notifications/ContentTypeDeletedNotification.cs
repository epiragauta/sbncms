using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class ContentTypeDeletedNotification : DeletedNotification<IContentType>
    {
        public ContentTypeDeletedNotification(IContentType target, EventMessages messages) : base(target, messages)
        {
        }

        public ContentTypeDeletedNotification(IEnumerable<IContentType> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
