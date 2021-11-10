using Microsoft.Extensions.DependencyInjection;
using Sbn.Cms.Core.DependencyInjection;
using Sbn.Cms.Web.Common.ModelsBuilder;
using Sbn.Extensions;

namespace Sbn.Cms.Web.BackOffice.ModelsBuilder
{
    /// <summary>
    /// Extension methods for <see cref="ISbnBuilder"/> for the common Sbn functionality
    /// </summary>
    public static class SbnBuilderExtensions
    {
        /// <summary>
        /// Adds the ModelsBuilder dashboard.
        /// </summary>
        public static ISbnBuilder AddModelsBuilderDashboard(this ISbnBuilder builder)
        {
            builder.Services.AddUnique<IModelsBuilderDashboardProvider, ModelsBuilderDashboardProvider>();
            return builder;
        }

        /// <summary>
        /// Can be called if using an external models builder to remove the embedded models builder controller features
        /// </summary>
        public static ISbnBuilder DisableModelsBuilderControllers(this ISbnBuilder builder)
        {
            builder.Services.AddSingleton<DisableModelsBuilderNotificationHandler>();
            return builder;
        }
    }
}
