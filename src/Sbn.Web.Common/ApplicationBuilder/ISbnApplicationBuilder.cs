using System;

namespace Sbn.Cms.Web.Common.ApplicationBuilder
{
    public interface ISbnApplicationBuilder
    {
        /// <summary>
        /// EXPERT call to replace the middlewares that Sbn installs by default with a completely custom pipeline.
        /// </summary>
        /// <param name="configureSbnMiddleware"></param>
        /// <returns></returns>
        ISbnEndpointBuilder WithCustomMiddleware(Action<ISbnApplicationBuilderContext> configureSbnMiddleware);

        /// <summary>
        /// Called to include default middleware to run sbn.
        /// </summary>
        /// <param name="configureSbnMiddleware"></param>
        /// <returns></returns>
        ISbnEndpointBuilder WithMiddleware(Action<ISbnApplicationBuilderContext> configureSbnMiddleware);
    }
}
