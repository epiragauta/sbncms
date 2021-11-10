// Copyright (c) Sbn.
// See LICENSE for more details.

using Microsoft.Extensions.Logging;
using Sbn.Cms.Core;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Logging;
using Sbn.Cms.Core.Notifications;

namespace Sbn.Cms.Web.Common.Profiler
{
    /// <summary>
    /// Initialized the web profiling. Ensures the boot process profiling is stopped.
    /// </summary>
    public class InitializeWebProfiling : INotificationHandler<SbnApplicationStartingNotification>
    {
        private readonly bool _profile;
        private readonly WebProfiler _profiler;

        /// <summary>
        /// Initializes a new instance of the <see cref="InitializeWebProfiling"/> class.
        /// </summary>
        public InitializeWebProfiling(IProfiler profiler, ILogger<InitializeWebProfiling> logger)
        {
            _profile = true;

            // although registered in SbnBuilderExtensions.AddSbn, ensure that we have not
            // been replaced by another component, and we are still "the" profiler
            _profiler = profiler as WebProfiler;
            if (_profiler != null)
            {
                return;
            }

            // if VoidProfiler was registered, let it be known
            if (profiler is NoopProfiler)
            {
                logger.LogInformation(
                    "Profiler is VoidProfiler, not profiling (must run debug mode to profile).");
            }

            _profile = false;
        }

        /// <inheritdoc/>
        public void Handle(SbnApplicationStartingNotification notification)
        {
            if (_profile && notification.RuntimeLevel == RuntimeLevel.Run)
            {
                // Stop the profiling of the booting process
                _profiler.StopBoot();
            }
        }

    }
}
