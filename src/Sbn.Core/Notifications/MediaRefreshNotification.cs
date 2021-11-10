using System;
using System.ComponentModel;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    [Obsolete("This is only used for the internal cache and will change, use tree change notifications instead")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class MediaRefreshNotification : EntityRefreshNotification<IMedia>
    {
        public MediaRefreshNotification(IMedia target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
