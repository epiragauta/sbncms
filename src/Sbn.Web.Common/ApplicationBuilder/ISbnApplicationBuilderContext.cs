using System;

namespace Sbn.Cms.Web.Common.ApplicationBuilder
{
    /// <summary>
    /// The context object used during 
    /// </summary>
    public interface ISbnApplicationBuilderContext : ISbnApplicationBuilderServices
    {
        /// <summary>
        /// Called to include the core sbn middleware.
        /// </summary>
        void UseSbnCoreMiddleware();

        /// <summary>
        /// Manually runs the <see cref="ISbnPipelineFilter"/> pre pipeline filters
        /// </summary>
        void RunPrePipeline();

        /// <summary>
        /// Manually runs the <see cref="ISbnPipelineFilter "/> post pipeline filters
        /// </summary>
        void RunPostPipeline();

        /// <summary>
        /// Called to include all of the default sbn required middleware.
        /// </summary>
        /// <remarks>
        /// If using this method, there is no need to use <see cref="UseSbnCoreMiddleware"/>
        /// </remarks>
        void RegisterDefaultRequiredMiddleware();
    }
}
