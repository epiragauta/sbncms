// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public sealed class ContentUnpublishingNotification : CancelableEnumerableObjectNotification<IContent>
    {
        public ContentUnpublishingNotification(IContent target, EventMessages messages) : base(target, messages)
        {
        }

        public ContentUnpublishingNotification(IEnumerable<IContent> target, EventMessages messages) : base(target, messages)
        {
        }

        public IEnumerable<IContent> UnpublishedEntities => Target;
    }
}
