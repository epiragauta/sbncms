// Copyright (c) Sbn.
// See LICENSE for more details.

namespace Sbn.Cms.Core.Notifications
{
    /// <summary>
    /// Notification that occurs at the very end of the Sbn boot
    /// process and after all <see cref="IComponent"/> initialize.
    /// </summary>
    public class SbnApplicationStartingNotification : INotification
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SbnApplicationStartingNotification"/> class.
        /// </summary>
        /// <param name="runtimeLevel">The runtime level</param>
        public SbnApplicationStartingNotification(RuntimeLevel runtimeLevel) => RuntimeLevel = runtimeLevel;

        /// <summary>
        /// Gets the runtime level of execution.
        /// </summary>
        public RuntimeLevel RuntimeLevel { get; }
    }
}
