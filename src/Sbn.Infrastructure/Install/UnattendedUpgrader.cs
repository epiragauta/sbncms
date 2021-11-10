using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sbn.Cms.Core.Configuration;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Exceptions;
using Sbn.Cms.Core.Logging;
using Sbn.Cms.Core.Notifications;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Infrastructure.Migrations.Install;
using Sbn.Cms.Infrastructure.Migrations.Upgrade;
using Sbn.Cms.Infrastructure.Runtime;
using Sbn.Extensions;
using Sbn.Cms.Core;
using Sbn.Cms.Infrastructure.Migrations;

namespace Sbn.Cms.Infrastructure.Install
{
    /// <summary>
    /// Handles <see cref="RuntimeUnattendedUpgradeNotification"/> to execute the unattended Sbn upgrader
    /// or the unattended Package migrations runner.
    /// </summary>
    public class UnattendedUpgrader : INotificationAsyncHandler<RuntimeUnattendedUpgradeNotification>
    {
        private readonly IProfilingLogger _profilingLogger;
        private readonly ISbnVersion _sbnVersion;
        private readonly DatabaseBuilder _databaseBuilder;
        private readonly IRuntimeState _runtimeState;
        private readonly PackageMigrationRunner _packageMigrationRunner;

        public UnattendedUpgrader(
            IProfilingLogger profilingLogger,
            ISbnVersion sbnVersion,
            DatabaseBuilder databaseBuilder,
            IRuntimeState runtimeState,
            PackageMigrationRunner packageMigrationRunner)
        {
            _profilingLogger = profilingLogger ?? throw new ArgumentNullException(nameof(profilingLogger));
            _sbnVersion = sbnVersion ?? throw new ArgumentNullException(nameof(sbnVersion));
            _databaseBuilder = databaseBuilder ?? throw new ArgumentNullException(nameof(databaseBuilder));
            _runtimeState = runtimeState ?? throw new ArgumentNullException(nameof(runtimeState));
            _packageMigrationRunner = packageMigrationRunner;
        }

        public Task HandleAsync(RuntimeUnattendedUpgradeNotification notification, CancellationToken cancellationToken)
        {
            if (_runtimeState.RunUnattendedBootLogic())
            {
                switch (_runtimeState.Reason)
                {
                    case RuntimeLevelReason.UpgradeMigrations:
                    {
                        var plan = new SbnPlan(_sbnVersion);
                        using (_profilingLogger.TraceDuration<UnattendedUpgrader>(
                            "Starting unattended upgrade.",
                            "Unattended upgrade completed."))
                        {
                            DatabaseBuilder.Result result = _databaseBuilder.UpgradeSchemaAndData(plan);
                            if (result.Success == false)
                            {
                                var innerException = new UnattendedInstallException("An error occurred while running the unattended upgrade.\n" + result.Message);
                                _runtimeState.Configure(Core.RuntimeLevel.BootFailed, Core.RuntimeLevelReason.BootFailedOnException, innerException);
                            }

                            notification.UnattendedUpgradeResult = RuntimeUnattendedUpgradeNotification.UpgradeResult.CoreUpgradeComplete;
                        }
                    }
                    break;
                    case RuntimeLevelReason.UpgradePackageMigrations:
                    {
                        if (!_runtimeState.StartupState.TryGetValue(RuntimeState.PendingPacakgeMigrationsStateKey, out var pm)
                            || pm is not IReadOnlyList<string> pendingMigrations)
                        {
                            throw new InvalidOperationException($"The required key {RuntimeState.PendingPacakgeMigrationsStateKey} does not exist in startup state");
                        }

                        if (pendingMigrations.Count == 0)
                        {
                            throw new InvalidOperationException("No pending migrations found but the runtime level reason is " + Core.RuntimeLevelReason.UpgradePackageMigrations);
                        }

                        try
                        {
                            IEnumerable<ExecutedMigrationPlan> result = _packageMigrationRunner.RunPackagePlans(pendingMigrations);
                            notification.UnattendedUpgradeResult = RuntimeUnattendedUpgradeNotification.UpgradeResult.PackageMigrationComplete;
                        }
                        catch (Exception ex )
                        {
                            SetRuntimeError(ex);
                            notification.UnattendedUpgradeResult = RuntimeUnattendedUpgradeNotification.UpgradeResult.HasErrors;
                        }
                    }
                    break;
                    default:
                        throw new InvalidOperationException("Invalid reason " + _runtimeState.Reason);
                }
            }

            return Task.CompletedTask;
        }

        private void SetRuntimeError(Exception exception)
            => _runtimeState.Configure(
                RuntimeLevel.BootFailed,
                RuntimeLevelReason.BootFailedOnException,
                exception);
    }
}
