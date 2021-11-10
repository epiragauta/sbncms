using System;
using System.Collections.Generic;
using System.ComponentModel;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Services.Changes;

namespace Sbn.Cms.Core.Notifications
{
    [Obsolete("This is only used for the internal cache and will change, use saved notifications instead")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class ContentTypeRefreshedNotification : ContentTypeRefreshNotification<IContentType>
    {
        public ContentTypeRefreshedNotification(ContentTypeChange<IContentType> target, EventMessages messages) : base(target, messages)
        {
        }

        public ContentTypeRefreshedNotification(IEnumerable<ContentTypeChange<IContentType>> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
