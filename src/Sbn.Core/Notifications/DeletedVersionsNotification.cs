// Copyright (c) Sbn.
// See LICENSE for more details.

using System;
using Sbn.Cms.Core.Events;

namespace Sbn.Cms.Core.Notifications
{
    public abstract class DeletedVersionsNotification<T> : DeletedVersionsNotificationBase<T> where T : class
    {
        protected DeletedVersionsNotification(int id, EventMessages messages, int specificVersion = default, bool deletePriorVersions = false, DateTime dateToRetain = default)
            : base(id, messages, specificVersion, deletePriorVersions, dateToRetain)
        {
        }
    }
}
