// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public sealed class ContentPublishedNotification : EnumerableObjectNotification<IContent>
    {
        public ContentPublishedNotification(IContent target, EventMessages messages) : base(target, messages)
        {
        }

        public ContentPublishedNotification(IEnumerable<IContent> target, EventMessages messages) : base(target, messages)
        {
        }

        public IEnumerable<IContent> PublishedEntities => Target;
    }
}
