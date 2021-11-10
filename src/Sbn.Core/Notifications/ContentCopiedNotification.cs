// Copyright (c) Sbn.
// See LICENSE for more details.

using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public sealed class ContentCopiedNotification : CopiedNotification<IContent>
    {
        public ContentCopiedNotification(IContent original, IContent copy, int parentId, bool relateToOriginal, EventMessages messages)
            : base(original, copy, parentId, relateToOriginal, messages)
        {
        }
    }
}
