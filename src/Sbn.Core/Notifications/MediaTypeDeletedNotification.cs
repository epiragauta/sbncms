using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class MediaTypeDeletedNotification : DeletedNotification<IMediaType>
    {
        public MediaTypeDeletedNotification(IMediaType target, EventMessages messages) : base(target, messages)
        {
        }

        public MediaTypeDeletedNotification(IEnumerable<IMediaType> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
