using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;
using Sbn.Cms.Core.Cache;
using Sbn.Cms.Core.Composing;
using Sbn.Cms.Core.Logging;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Web;
using Sbn.Cms.Core.Web.Mvc;
using Sbn.Cms.Infrastructure.Persistence;
using Sbn.Cms.Web.Common.Attributes;
using Sbn.Extensions;

namespace Sbn.Cms.Web.Common.Controllers
{
    /// <summary>
    /// Provides a base class for plugin controllers.
    /// </summary>
    public abstract class PluginController : Controller, IDiscoverable
    {
        private static readonly ConcurrentDictionary<Type, PluginControllerMetadata> MetadataStorage
            = new ConcurrentDictionary<Type, PluginControllerMetadata>();

        // for debugging purposes
        internal Guid InstanceId { get; } = Guid.NewGuid();

        /// <summary>
        /// Gets the Sbn context.
        /// </summary>
        public virtual ISbnContext SbnContext
        {
            get
            {
                var sbnContext = SbnContextAccessor.GetRequiredSbnContext();
                return sbnContext;
            }
        }

        /// <summary>
        /// Gets the database context accessor.
        /// </summary>
        public virtual ISbnContextAccessor SbnContextAccessor { get; }

        /// <summary>
        /// Gets the database context.
        /// </summary>
        public ISbnDatabaseFactory DatabaseFactory { get; }

        /// <summary>
        /// Gets or sets the services context.
        /// </summary>
        public ServiceContext Services { get; }

        /// <summary>
        /// Gets or sets the application cache.
        /// </summary>
        public AppCaches AppCaches { get;  }

        /// <summary>
        /// Gets or sets the profiling logger.
        /// </summary>
        public IProfilingLogger ProfilingLogger { get; }

        /// <summary>
        /// Gets metadata for this instance.
        /// </summary>
        internal PluginControllerMetadata Metadata => GetMetadata(GetType());

        protected PluginController(ISbnContextAccessor sbnContextAccessor, ISbnDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger)
        {
            SbnContextAccessor = sbnContextAccessor;
            DatabaseFactory = databaseFactory;
            Services = services;
            AppCaches = appCaches;
            ProfilingLogger = profilingLogger;
        }

        /// <summary>
        /// Gets metadata for a controller type.
        /// </summary>
        /// <param name="controllerType">The controller type.</param>
        /// <returns>Metadata for the controller type.</returns>
        public static PluginControllerMetadata GetMetadata(Type controllerType)
        {
            return MetadataStorage.GetOrAdd(controllerType, type =>
            {
                // plugin controller? back-office controller?
                var pluginAttribute = controllerType.GetCustomAttribute<PluginControllerAttribute>(false);
                var backOfficeAttribute = controllerType.GetCustomAttribute<IsBackOfficeAttribute>(true);

                return new PluginControllerMetadata
                {
                    AreaName = pluginAttribute?.AreaName,
                    ControllerName = ControllerExtensions.GetControllerName(controllerType),
                    ControllerNamespace = controllerType.Namespace,
                    ControllerType = controllerType,
                    IsBackOffice = backOfficeAttribute != null
                };
            });
        }
    }
}
