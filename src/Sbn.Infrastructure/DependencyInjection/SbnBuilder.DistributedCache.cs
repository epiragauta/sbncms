using System;
using Microsoft.Extensions.DependencyInjection;
using Sbn.Cms.Core.Cache;
using Sbn.Cms.Core.DependencyInjection;
using Sbn.Cms.Core.Notifications;
using Sbn.Cms.Core.Sync;
using Sbn.Cms.Infrastructure.Sync;
using Sbn.Extensions;

namespace Sbn.Cms.Infrastructure.DependencyInjection
{
    /// <summary>
    /// Provides extension methods to the <see cref="ISbnBuilder"/> class.
    /// </summary>
    public static partial class SbnBuilderExtensions
    {
        /// <summary>
        /// Adds distributed cache support
        /// </summary>
        /// <remarks>
        /// This is still required for websites that are not load balancing because this ensures that sites hosted
        /// with managed hosts like IIS/etc... work correctly when AppDomains are running in parallel.
        /// </remarks>
        public static ISbnBuilder AddDistributedCache(this ISbnBuilder builder)
        {
            builder.Services.AddSingleton<LastSyncedFileManager>();
            builder.Services.AddSingleton<ISyncBootStateAccessor, SyncBootStateAccessor>();
            builder.SetServerMessenger<BatchedDatabaseServerMessenger>();
            builder.AddNotificationHandler<SbnApplicationStartingNotification, DatabaseServerMessengerNotificationHandler>();
            builder.AddNotificationHandler<SbnRequestEndNotification, DatabaseServerMessengerNotificationHandler>();
            return builder;
        }

        /// <summary>
        /// Sets the server registrar.
        /// </summary>
        /// <typeparam name="T">The type of the server registrar.</typeparam>
        /// <param name="builder">The builder.</param>
        public static ISbnBuilder SetServerRegistrar<T>(this ISbnBuilder builder)
            where T : class, IServerRoleAccessor
        {
            builder.Services.AddUnique<IServerRoleAccessor, T>();
            return builder;
        }

        /// <summary>
        /// Sets the server registrar.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="factory">A function creating a server registrar.</param>
        public static ISbnBuilder SetServerRegistrar(this ISbnBuilder builder, Func<IServiceProvider, IServerRoleAccessor> factory)
        {
            builder.Services.AddUnique(factory);
            return builder;
        }

        /// <summary>
        /// Sets the server registrar.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="registrar">A server registrar.</param>
        public static ISbnBuilder SetServerRegistrar(this ISbnBuilder builder, IServerRoleAccessor registrar)
        {
            builder.Services.AddUnique(registrar);
            return builder;
        }

        /// <summary>
        /// Sets the server messenger.
        /// </summary>
        /// <typeparam name="T">The type of the server registrar.</typeparam>
        /// <param name="builder">The builder.</param>
        public static ISbnBuilder SetServerMessenger<T>(this ISbnBuilder builder)
            where T : class, IServerMessenger
        {
            builder.Services.AddUnique<IServerMessenger, T>();
            return builder;
        }

        /// <summary>
        /// Sets the server messenger.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="factory">A function creating a server messenger.</param>
        public static ISbnBuilder SetServerMessenger(this ISbnBuilder builder, Func<IServiceProvider, IServerMessenger> factory)
        {
            builder.Services.AddUnique(factory);
            return builder;
        }

        /// <summary>
        /// Sets the server messenger.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="registrar">A server messenger.</param>
        public static ISbnBuilder SetServerMessenger(this ISbnBuilder builder, IServerMessenger registrar)
        {
            builder.Services.AddUnique(registrar);
            return builder;
        }
    }
}
