// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Threading;
using System.Threading.Tasks;
using Sbn.Cms.Core.Notifications;

namespace Sbn.Cms.Core.Events
{
    /// <summary>
    /// Defines a handler for a async notification.
    /// </summary>
    /// <typeparam name="TNotification">The type of notification being handled.</typeparam>
    public interface INotificationAsyncHandler<in TNotification>
        where TNotification : INotification
    {
        /// <summary>
        /// Handles a notification
        /// </summary>
        /// <param name="notification">The notification</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task HandleAsync(TNotification notification, CancellationToken cancellationToken);
    }
}
