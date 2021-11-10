using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public class MediaTypeSavingNotification : SavingNotification<IMediaType>
    {
        public MediaTypeSavingNotification(IMediaType target, EventMessages messages) : base(target, messages)
        {
        }

        public MediaTypeSavingNotification(IEnumerable<IMediaType> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
