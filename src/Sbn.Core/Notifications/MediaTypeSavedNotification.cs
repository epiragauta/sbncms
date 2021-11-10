using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class MediaTypeSavedNotification : SavedNotification<IMediaType>
    {
        public MediaTypeSavedNotification(IMediaType target, EventMessages messages) : base(target, messages)
        {
        }

        public MediaTypeSavedNotification(IEnumerable<IMediaType> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
