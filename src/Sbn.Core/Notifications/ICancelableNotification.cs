// Copyright (c) Sbn.
// See LICENSE for more details.

namespace Sbn.Cms.Core.Notifications
{
    public interface ICancelableNotification : INotification
    {
        bool Cancel { get; set; }
    }
}
