using System;
using Smidge;
using Smidge.Nuglify;
using Sbn.Cms.Web.Common.ApplicationBuilder;
using Sbn.Extensions;

namespace Sbn.Cms.Web.Common.Extensions
{
    public static partial class SbnApplicationBuilderExtensions
    {
        /// <summary>
        /// Enables runtime minification for Sbn
        /// </summary>
        public static ISbnEndpointBuilderContext UseSbnRuntimeMinificationEndpoints(this ISbnEndpointBuilderContext app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (!app.RuntimeState.SbnCanBoot())
            {
                return app;
            }

            app.AppBuilder.UseSmidge();
            app.AppBuilder.UseSmidgeNuglify();

            return app;
        }
    }
}
