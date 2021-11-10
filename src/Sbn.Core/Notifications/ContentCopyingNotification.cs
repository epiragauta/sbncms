// Copyright (c) Sbn.
// See LICENSE for more details.

using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public sealed class ContentCopyingNotification : CopyingNotification<IContent>
    {
        public ContentCopyingNotification(IContent original, IContent copy, int parentId, EventMessages messages)
            : base(original, copy, parentId, messages)
        {
        }
    }
}
