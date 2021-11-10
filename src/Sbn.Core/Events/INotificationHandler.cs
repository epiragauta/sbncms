// Copyright (c) Sbn.
// See LICENSE for more details.

using Sbn.Cms.Core.Notifications;

namespace Sbn.Cms.Core.Events
{
    /// <summary>
    /// Defines a handler for a notification.
    /// </summary>
    /// <typeparam name="TNotification">The type of notification being handled.</typeparam>
    public interface INotificationHandler<in TNotification>
        where TNotification : INotification
    {
        /// <summary>
        /// Handles a notification
        /// </summary>
        /// <param name="notification">The notification</param>
        void Handle(TNotification notification);
    }
}
