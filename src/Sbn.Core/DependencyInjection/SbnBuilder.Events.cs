// Copyright (c) Sbn.
// See LICENSE for more details.

using Microsoft.Extensions.DependencyInjection;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Notifications;

namespace Sbn.Cms.Core.DependencyInjection
{

    /// <summary>
    /// Contains extensions methods for <see cref="ISbnBuilder"/> used for registering event handlers.
    /// </summary>
    public static partial class SbnBuilderExtensions
    {
        /// <summary>
        /// Registers a notification handler against the Sbn service collection.
        /// </summary>
        /// <typeparam name="TNotification">The type of notification.</typeparam>
        /// <typeparam name="TNotificationHandler">The type of notificiation handler.</typeparam>
        /// <param name="builder">The Sbn builder.</param>
        /// <returns>The <see cref="ISbnBuilder"/>.</returns>
        public static ISbnBuilder AddNotificationHandler<TNotification, TNotificationHandler>(this ISbnBuilder builder)
            where TNotificationHandler : INotificationHandler<TNotification>
            where TNotification : INotification
        {
            builder.Services.AddNotificationHandler<TNotification, TNotificationHandler>();
            return builder;
        }

        /// <summary>
        /// Registers a notification async handler against the Sbn service collection.
        /// </summary>
        /// <typeparam name="TNotification">The type of notification.</typeparam>
        /// <typeparam name="TNotificationAsyncHandler">The type of notification async handler.</typeparam>
        /// <param name="builder">The Sbn builder.</param>
        /// <returns>The <see cref="ISbnBuilder"/>.</returns>
        public static ISbnBuilder AddNotificationAsyncHandler<TNotification, TNotificationAsyncHandler>(this ISbnBuilder builder)
            where TNotificationAsyncHandler : INotificationAsyncHandler<TNotification>
            where TNotification : INotification
        {
            builder.Services.AddNotificationAsyncHandler<TNotification, TNotificationAsyncHandler>();
            return builder;
        }

        internal static IServiceCollection AddNotificationHandler<TNotification, TNotificationHandler>(this IServiceCollection services)
            where TNotificationHandler : INotificationHandler<TNotification>
            where TNotification : INotification
        {
            // Register the handler as transient. This ensures that anything can be injected into it.
            var descriptor = new UniqueServiceDescriptor(typeof(INotificationHandler<TNotification>), typeof(TNotificationHandler), ServiceLifetime.Transient);

            if (!services.Contains(descriptor))
            {
                services.Add(descriptor);
            }

            return services;
        }

        internal static IServiceCollection AddNotificationAsyncHandler<TNotification, TNotificationAsyncHandler>(this IServiceCollection services)
            where TNotificationAsyncHandler : INotificationAsyncHandler<TNotification>
            where TNotification : INotification
        {
            // Register the handler as transient. This ensures that anything can be injected into it.
            var descriptor = new ServiceDescriptor(typeof(INotificationAsyncHandler<TNotification>), typeof(TNotificationAsyncHandler), ServiceLifetime.Transient);

            if (!services.Contains(descriptor))
            {
                services.Add(descriptor);
            }

            return services;
        }
    }
}
