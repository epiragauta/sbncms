using System;
using System.ComponentModel;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{

    [Obsolete("This is only used for the internal cache and will change, use saved notifications instead")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class ContentRefreshNotification : EntityRefreshNotification<IContent>
    {
        public ContentRefreshNotification(IContent target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
