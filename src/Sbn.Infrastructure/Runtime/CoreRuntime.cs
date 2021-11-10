using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sbn.Cms.Core;
using Sbn.Cms.Core.Configuration;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Exceptions;
using Sbn.Cms.Core.Hosting;
using Sbn.Cms.Core.Logging;
using Sbn.Cms.Core.Notifications;
using Sbn.Cms.Core.Runtime;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Infrastructure.Persistence;
using Sbn.Cms.Web.Common.DependencyInjection;
using Sbn.Extensions;
using ComponentCollection = Sbn.Cms.Core.Composing.ComponentCollection;

namespace Sbn.Cms.Infrastructure.Runtime
{
    public class CoreRuntime : IRuntime
    {
        private readonly ILogger<CoreRuntime> _logger;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ComponentCollection _components;
        private readonly IApplicationShutdownRegistry _applicationShutdownRegistry;
        private readonly IProfilingLogger _profilingLogger;
        private readonly IMainDom _mainDom;
        private readonly ISbnDatabaseFactory _databaseFactory;
        private readonly IEventAggregator _eventAggregator;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ISbnVersion _sbnVersion;
        private readonly IServiceProvider _serviceProvider;
        private CancellationToken _cancellationToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="CoreRuntime"/> class.
        /// </summary>
        public CoreRuntime(
            ILoggerFactory loggerFactory,
            IRuntimeState state,
            ComponentCollection components,
            IApplicationShutdownRegistry applicationShutdownRegistry,
            IProfilingLogger profilingLogger,
            IMainDom mainDom,
            ISbnDatabaseFactory databaseFactory,
            IEventAggregator eventAggregator,
            IHostingEnvironment hostingEnvironment,
            ISbnVersion sbnVersion,
            IServiceProvider serviceProvider)
        {
            State = state;
            _loggerFactory = loggerFactory;
            _components = components;
            _applicationShutdownRegistry = applicationShutdownRegistry;
            _profilingLogger = profilingLogger;
            _mainDom = mainDom;
            _databaseFactory = databaseFactory;
            _eventAggregator = eventAggregator;
            _hostingEnvironment = hostingEnvironment;
            _sbnVersion = sbnVersion;
            _serviceProvider = serviceProvider;
            _logger = _loggerFactory.CreateLogger<CoreRuntime>();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete]
        public CoreRuntime(
            ILoggerFactory loggerFactory,
            IRuntimeState state,
            ComponentCollection components,
            IApplicationShutdownRegistry applicationShutdownRegistry,
            IProfilingLogger profilingLogger,
            IMainDom mainDom,
            ISbnDatabaseFactory databaseFactory,
            IEventAggregator eventAggregator,
            IHostingEnvironment hostingEnvironment,
            ISbnVersion sbnVersion
            ):this(
            loggerFactory,
            state,
            components,
            applicationShutdownRegistry,
            profilingLogger,
            mainDom,
            databaseFactory,
            eventAggregator,
            hostingEnvironment,
            sbnVersion,
            null
            )
        {

        }

        /// <summary>
        /// Gets the state of the Sbn runtime.
        /// </summary>
        public IRuntimeState State { get; }

        /// <inheritdoc/>
        public async Task RestartAsync()
        {
            await StopAsync(_cancellationToken);
            await StartAsync(_cancellationToken);
        }

        /// <inheritdoc/>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            StaticApplicationLogging.Initialize(_loggerFactory);
            StaticServiceProvider.Instance = _serviceProvider;

            AppDomain.CurrentDomain.UnhandledException += (_, args) =>
            {
                var exception = (Exception)args.ExceptionObject;
                var isTerminating = args.IsTerminating; // always true?

                var msg = "Unhandled exception in AppDomain";

                if (isTerminating)
                {
                    msg += " (terminating)";
                }

                msg += ".";

                _logger.LogError(exception, msg);
            };

            // acquire the main domain - if this fails then anything that should be registered with MainDom will not operate
            AcquireMainDom();

            await _eventAggregator.PublishAsync(new SbnApplicationMainDomAcquiredNotification(), cancellationToken);

            // notify for unattended install
            await _eventAggregator.PublishAsync(new RuntimeUnattendedInstallNotification());
            DetermineRuntimeLevel();

            if (!State.SbnCanBoot())
            {
                return; // The exception will be rethrown by BootFailedMiddelware
            }

            IApplicationShutdownRegistry hostingEnvironmentLifetime = _applicationShutdownRegistry;
            if (hostingEnvironmentLifetime == null)
            {
                throw new InvalidOperationException($"An instance of {typeof(IApplicationShutdownRegistry)} could not be resolved from the container, ensure that one if registered in your runtime before calling {nameof(IRuntime)}.{nameof(StartAsync)}");
            }

            // if level is Run and reason is UpgradeMigrations, that means we need to perform an unattended upgrade
            var unattendedUpgradeNotification = new RuntimeUnattendedUpgradeNotification();
            await _eventAggregator.PublishAsync(unattendedUpgradeNotification);
            switch (unattendedUpgradeNotification.UnattendedUpgradeResult)
            {
                case RuntimeUnattendedUpgradeNotification.UpgradeResult.HasErrors:
                    if (State.BootFailedException == null)
                    {
                        throw new InvalidOperationException($"Unattended upgrade result was {RuntimeUnattendedUpgradeNotification.UpgradeResult.HasErrors} but no {nameof(BootFailedException)} was registered");
                    }
                    // we cannot continue here, the exception will be rethrown by BootFailedMiddelware
                    return;
                case RuntimeUnattendedUpgradeNotification.UpgradeResult.CoreUpgradeComplete:
                case RuntimeUnattendedUpgradeNotification.UpgradeResult.PackageMigrationComplete:
                    // upgrade is done, set reason to Run
                    DetermineRuntimeLevel();
                    break;
                case RuntimeUnattendedUpgradeNotification.UpgradeResult.NotRequired:
                    break;
            }

            await _eventAggregator.PublishAsync(new SbnApplicationComponentsInstallingNotification(State.Level), cancellationToken);

            // create & initialize the components
            _components.Initialize();

            await _eventAggregator.PublishAsync(new SbnApplicationStartingNotification(State.Level), cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _components.Terminate();
            await _eventAggregator.PublishAsync(new SbnApplicationStoppingNotification(), cancellationToken);
            StaticApplicationLogging.Initialize(null);
        }

        private void AcquireMainDom()
        {
            using (DisposableTimer timer = _profilingLogger.DebugDuration<CoreRuntime>("Acquiring MainDom.", "Acquired."))
            {
                try
                {
                    _mainDom.Acquire(_applicationShutdownRegistry);
                }
                catch
                {
                    timer?.Fail();
                    throw;
                }
            }
        }

        private void DetermineRuntimeLevel()
        {
            if (State.BootFailedException != null)
            {
                // there's already been an exception so cannot boot and no need to check
                return;
            }

            using DisposableTimer timer = _profilingLogger.DebugDuration<CoreRuntime>("Determining runtime level.", "Determined.");

            try
            {
                State.DetermineRuntimeLevel();

                _logger.LogDebug("Runtime level: {RuntimeLevel} - {RuntimeLevelReason}", State.Level, State.Reason);

                if (State.Level == RuntimeLevel.Upgrade)
                {
                    _logger.LogDebug("Configure database factory for upgrades.");
                    _databaseFactory.ConfigureForUpgrade();
                }
            }
            catch (Exception ex)
            {
                State.Configure(RuntimeLevel.BootFailed, RuntimeLevelReason.BootFailedOnException);
                timer?.Fail();
                _logger.LogError(ex, "Boot Failed");
                // We do not throw the exception. It will be rethrown by BootFailedMiddleware
            }
        }
    }
}
