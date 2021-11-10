using Sbn.Cms.Core.Routing;

namespace Sbn.Cms.Core.Notifications
{
    /// <summary>
    /// Used for notifying when an Sbn request is being built
    /// </summary>
    public class RoutingRequestNotification : INotification
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoutingRequestNotification"/> class.
        /// </summary>
        public RoutingRequestNotification(IPublishedRequestBuilder requestBuilder) => RequestBuilder = requestBuilder;

        /// <summary>
        /// Gets the <see cref="IPublishedRequestBuilder"/>
        /// </summary>
        public IPublishedRequestBuilder RequestBuilder { get; }
    }
}
