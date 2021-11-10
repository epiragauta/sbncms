using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Services.Changes;

namespace Sbn.Cms.Core.Notifications
{
    public class MediaTypeChangedNotification : ContentTypeChangeNotification<IMediaType>
    {
        public MediaTypeChangedNotification(ContentTypeChange<IMediaType> target, EventMessages messages) : base(target, messages)
        {
        }

        public MediaTypeChangedNotification(IEnumerable<ContentTypeChange<IMediaType>> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
