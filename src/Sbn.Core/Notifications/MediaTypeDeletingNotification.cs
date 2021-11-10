using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class MediaTypeDeletingNotification : DeletingNotification<IMediaType>
    {
        public MediaTypeDeletingNotification(IMediaType target, EventMessages messages) : base(target, messages)
        {
        }

        public MediaTypeDeletingNotification(IEnumerable<IMediaType> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
