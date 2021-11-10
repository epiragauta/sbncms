// Copyright (c) Sbn.
// See LICENSE for more details.

using Sbn.Cms.Core.Web;

namespace Sbn.Cms.Core.Notifications
{
    /// <summary>
    /// Notification raised on each request begin.
    /// </summary>
    public class SbnRequestBeginNotification : INotification
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SbnRequestBeginNotification"/> class.
        /// </summary>
        public SbnRequestBeginNotification(ISbnContext sbnContext) => SbnContext = sbnContext;

        /// <summary>
        /// Gets the <see cref="ISbnContext"/>
        /// </summary>
        public ISbnContext SbnContext { get; }
    }
}
