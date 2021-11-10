// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public sealed class MediaSavedNotification : SavedNotification<IMedia>
    {
        public MediaSavedNotification(IMedia target, EventMessages messages) : base(target, messages)
        {
        }

        public MediaSavedNotification(IEnumerable<IMedia> target, EventMessages messages) : base(target, messages)
        {
        }
    }
}
