// Copyright (c) Sbn.
// See LICENSE for more details.

namespace Sbn.Cms.Core.Notifications
{
    /// <summary>
    /// Notification that occurs during Sbn boot after the MainDom has been acquired.
    /// </summary>
    public class SbnApplicationMainDomAcquiredNotification : INotification
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SbnApplicationMainDomAcquiredNotification"/> class.
        /// </summary>
        /// <param name="runtimeLevel">The runtime level</param>
        public SbnApplicationMainDomAcquiredNotification()
        {
        }
    }
}
