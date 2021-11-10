using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Features;
using Sbn.Cms.Core.Notifications;

namespace Sbn.Cms.Web.BackOffice.ModelsBuilder
{
    /// <summary>
    /// Used in conjunction with <see cref="SbnBuilderExtensions.DisableModelsBuilderControllers"/>
    /// </summary>
    internal class DisableModelsBuilderNotificationHandler : INotificationHandler<SbnApplicationStartingNotification>
    {
        private readonly SbnFeatures _features;

        public DisableModelsBuilderNotificationHandler(SbnFeatures features) => _features = features;

        /// <summary>
        /// Handles the <see cref="SbnApplicationStartingNotification"/> notification to disable MB controller features
        /// </summary>
        public void Handle(SbnApplicationStartingNotification notification) =>
            // disable the embedded dashboard controller
            _features.Disabled.Controllers.Add<ModelsBuilderDashboardController>();
    }
}
