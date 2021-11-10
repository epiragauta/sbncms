// Copyright (c) Sbn.
// See LICENSE for more details.

namespace Sbn.Cms.Core.Notifications
{
    /// <summary>
    /// Notification that occurs during the Sbn boot process, before instances of <see cref="IComponent"/> initialize.
    /// </summary>
    public class SbnApplicationComponentsInstallingNotification : INotification
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SbnApplicationStartingNotification"/> class.
        /// </summary>
        /// <param name="runtimeLevel">The runtime level</param>
        public SbnApplicationComponentsInstallingNotification(RuntimeLevel runtimeLevel) => RuntimeLevel = runtimeLevel;

        /// <summary>
        /// Gets the runtime level of execution.
        /// </summary>
        public RuntimeLevel RuntimeLevel { get; }
    }
}
