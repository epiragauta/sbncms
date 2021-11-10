using System;

namespace Sbn.Cms.Web.Common.ApplicationBuilder
{
    public interface ISbnEndpointBuilder
    {
        /// <summary>
        /// Final call during app building to configure endpoints
        /// </summary>
        /// <param name="configureSbn"></param>
        void WithEndpoints(Action<ISbnEndpointBuilderContext> configureSbn);
    }
}
