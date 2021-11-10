using System;
using Sbn.Cms.Core.DependencyInjection;
using Sbn.Cms.Core.PublishedCache;

namespace Sbn.Extensions
{
    public static class CompositionExtensions
    {
        /// <summary>
        /// Sets the published snapshot service.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="factory">A function creating a published snapshot service.</param>
        public static ISbnBuilder SetPublishedSnapshotService(this ISbnBuilder builder, Func<IServiceProvider, IPublishedSnapshotService> factory)
        {
            builder.Services.AddUnique(factory);
            return builder;
        }

        /// <summary>
        /// Sets the published snapshot service.
        /// </summary>
        /// <typeparam name="T">The type of the published snapshot service.</typeparam>
        /// <param name="builder">The builder.</param>
        public static ISbnBuilder SetPublishedSnapshotService<T>(this ISbnBuilder builder)
            where T : class, IPublishedSnapshotService
        {
            builder.Services.AddUnique<IPublishedSnapshotService, T>();
            return builder;
        }

        /// <summary>
        /// Sets the published snapshot service.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="service">A published snapshot service.</param>
        public static ISbnBuilder SetPublishedSnapshotService(this ISbnBuilder builder, IPublishedSnapshotService service)
        {
            builder.Services.AddUnique(service);
            return builder;
        }
    }
}
