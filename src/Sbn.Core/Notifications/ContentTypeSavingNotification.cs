using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class ContentTypeSavingNotification : SavingNotification<IContentType>
    {
        public ContentTypeSavingNotification(IContentType target, EventMessages messages) : base(target, messages)
        {
        }

        public ContentTypeSavingNotification(IEnumerable<IContentType> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
