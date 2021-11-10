// Copyright (c) Sbn.
// See LICENSE for more details.

using System;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public sealed class MediaDeletedVersionsNotification : DeletedVersionsNotification<IMedia>
    {
        public MediaDeletedVersionsNotification(int id, EventMessages messages, int specificVersion = default, bool deletePriorVersions = false, DateTime dateToRetain = default) : base(id, messages, specificVersion, deletePriorVersions, dateToRetain)
        {
        }
    }
}
