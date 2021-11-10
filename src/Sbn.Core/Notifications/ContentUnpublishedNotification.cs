// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public sealed class ContentUnpublishedNotification : EnumerableObjectNotification<IContent>
    {
        public ContentUnpublishedNotification(IContent target, EventMessages messages) : base(target, messages)
        {
        }

        public ContentUnpublishedNotification(IEnumerable<IContent> target, EventMessages messages) : base(target, messages)
        {
        }

        public IEnumerable<IContent> UnpublishedEntities => Target;
    }
}
