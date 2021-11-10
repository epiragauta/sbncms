// Copyright (c) Sbn.
// See LICENSE for more details.

using Sbn.Cms.Core.Web;

namespace Sbn.Cms.Core.Notifications
{
    /// <summary>
    /// Notification raised on each request end.
    /// </summary>
    public class SbnRequestEndNotification : INotification
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SbnRequestEndNotification"/> class.
        /// </summary>
        public SbnRequestEndNotification(ISbnContext sbnContext) => SbnContext = sbnContext;

        /// <summary>
        /// Gets the <see cref="ISbnContext"/>
        /// </summary>
        public ISbnContext SbnContext { get; }
    }
}
