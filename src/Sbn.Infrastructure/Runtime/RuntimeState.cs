using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sbn.Cms.Core;
using Sbn.Cms.Core.Configuration;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.Exceptions;
using Sbn.Cms.Core.Packaging;
using Sbn.Cms.Core.Semver;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Infrastructure.Migrations.Upgrade;
using Sbn.Cms.Infrastructure.Persistence;
using Sbn.Extensions;

namespace Sbn.Cms.Infrastructure.Runtime
{

    /// <summary>
    /// Represents the state of the Sbn runtime.
    /// </summary>
    public class RuntimeState : IRuntimeState
    {
        internal const string PendingPacakgeMigrationsStateKey = "PendingPackageMigrations";
        private readonly IOptions<GlobalSettings> _globalSettings;
        private readonly IOptions<UnattendedSettings> _unattendedSettings;
        private readonly ISbnVersion _sbnVersion;
        private readonly ISbnDatabaseFactory _databaseFactory;
        private readonly ILogger<RuntimeState> _logger;
        private readonly PendingPackageMigrations _packageMigrationState;
        private readonly Dictionary<string, object> _startupState = new Dictionary<string, object>();

        /// <summary>
        /// The initial <see cref="RuntimeState"/>
        /// The initial <see cref="RuntimeState"/>
        /// </summary>
        public static RuntimeState Booting() => new RuntimeState() { Level = RuntimeLevel.Boot };

        private RuntimeState()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RuntimeState"/> class.
        /// </summary>
        public RuntimeState(
            IOptions<GlobalSettings> globalSettings,
            IOptions<UnattendedSettings> unattendedSettings,
            ISbnVersion sbnVersion,
            ISbnDatabaseFactory databaseFactory,
            ILogger<RuntimeState> logger,
            PendingPackageMigrations packageMigrationState)
        {
            _globalSettings = globalSettings;
            _unattendedSettings = unattendedSettings;
            _sbnVersion = sbnVersion;
            _databaseFactory = databaseFactory;
            _logger = logger;
            _packageMigrationState = packageMigrationState;
        }


        /// <inheritdoc />
        public Version Version => _sbnVersion.Version;

        /// <inheritdoc />
        public string VersionComment => _sbnVersion.Comment;

        /// <inheritdoc />
        public SemVersion SemanticVersion => _sbnVersion.SemanticVersion;

        /// <inheritdoc />
        public string CurrentMigrationState { get; private set; }

        /// <inheritdoc />
        public string FinalMigrationState { get; private set; }

        /// <inheritdoc />
        public RuntimeLevel Level { get; internal set; } = RuntimeLevel.Unknown;

        /// <inheritdoc />
        public RuntimeLevelReason Reason { get; internal set; } = RuntimeLevelReason.Unknown;

        /// <inheritdoc />
        public BootFailedException BootFailedException { get; internal set; }

        /// <inheritdoc />
        public IReadOnlyDictionary<string, object> StartupState => _startupState;

        /// <inheritdoc />
        public void DetermineRuntimeLevel()
        {
            if (_databaseFactory.Configured == false)
            {
                // local version *does* match code version, but the database is not configured
                // install - may happen with Deploy/Cloud/etc
                _logger.LogDebug("Database is not configured, need to install Sbn.");
                Level = RuntimeLevel.Install;
                Reason = RuntimeLevelReason.InstallNoDatabase;
                return;
            }

            // Check the database state, whether we can connect or if it's in an upgrade or empty state, etc...

            switch (GetSbnDatabaseState(_databaseFactory))
            {
                case SbnDatabaseState.CannotConnect:
                {
                    // cannot connect to configured database, this is bad, fail
                    _logger.LogDebug("Could not connect to database.");

                    if (_globalSettings.Value.InstallMissingDatabase || CanAutoInstallMissingDatabase(_databaseFactory))
                    {
                        // ok to install on a configured but missing database
                        Level = RuntimeLevel.Install;
                        Reason = RuntimeLevelReason.InstallMissingDatabase;
                        return;
                    }

                    // else it is bad enough that we want to throw
                    Reason = RuntimeLevelReason.BootFailedCannotConnectToDatabase;
                    BootFailedException = new BootFailedException("A connection string is configured but Sbn could not connect to the database.");
                    throw BootFailedException;
                }
                case SbnDatabaseState.NotInstalled:
                {
                    // ok to install on an empty database
                    Level = RuntimeLevel.Install;
                    Reason = RuntimeLevelReason.InstallEmptyDatabase;
                    return;
                }
                case SbnDatabaseState.NeedsUpgrade:
                {
                    // the db version does not match... but we do have a migration table
                    // so, at least one valid table, so we quite probably are installed & need to upgrade

                    // although the files version matches the code version, the database version does not
                    // which means the local files have been upgraded but not the database - need to upgrade
                    _logger.LogDebug("Has not reached the final upgrade step, need to upgrade Sbn.");
                    Level = _unattendedSettings.Value.UpgradeUnattended ? RuntimeLevel.Run : RuntimeLevel.Upgrade;
                    Reason = RuntimeLevelReason.UpgradeMigrations;
                }
                break;
                case SbnDatabaseState.NeedsPackageMigration:

                    // no matter what the level is run for package migrations.
                    // they either run unattended, or only manually via the back office.
                    Level = RuntimeLevel.Run;

                    if (_unattendedSettings.Value.PackageMigrationsUnattended)
                    {
                        _logger.LogDebug("Package migrations need to execute.");
                        Reason = RuntimeLevelReason.UpgradePackageMigrations;
                    }
                    else
                    {
                        _logger.LogInformation("Package migrations need to execute but unattended package migrations is disabled. They will need to be run from the back office.");
                        Reason = RuntimeLevelReason.Run;
                    }

                    break;
                case SbnDatabaseState.Ok:
                default:
                {


                    // the database version matches the code & files version, all clear, can run
                    Level = RuntimeLevel.Run;
                    Reason = RuntimeLevelReason.Run;
                }
                break;
            }
        }

        public void Configure(RuntimeLevel level, RuntimeLevelReason reason, Exception bootFailedException = null)
        {
            Level = level;
            Reason = reason;

            if (bootFailedException != null)
            {
                BootFailedException = new BootFailedException(bootFailedException.Message, bootFailedException);
            }
        }

        private enum SbnDatabaseState
        {
            Ok,
            CannotConnect,
            NotInstalled,
            NeedsUpgrade,
            NeedsPackageMigration
        }

        private SbnDatabaseState GetSbnDatabaseState(ISbnDatabaseFactory databaseFactory)
        {
            try
            {
                if (!TryDbConnect(databaseFactory))
                {
                    return SbnDatabaseState.CannotConnect;
                }

                // no scope, no service - just directly accessing the database
                using (var database = databaseFactory.CreateDatabase())
                {
                    if (!database.IsSbnInstalled())
                    {
                        return SbnDatabaseState.NotInstalled;
                    }

                    // Make ONE SQL call to determine Sbn upgrade vs package migrations state.
                    // All will be prefixed with the same key.
                    IReadOnlyDictionary<string, string> keyValues = database.GetFromKeyValueTable(Constants.Conventions.Migrations.KeyValuePrefix);

                    // This could need both an upgrade AND package migrations to execute but
                    // we will process them one at a time, first the upgrade, then the package migrations.
                    if (DoesSbnRequireUpgrade(keyValues))
                    {
                        return SbnDatabaseState.NeedsUpgrade;
                    }

                    IReadOnlyList<string> packagesRequiringMigration = _packageMigrationState.GetPendingPackageMigrations(keyValues);
                    if (packagesRequiringMigration.Count > 0)
                    {
                        _startupState[PendingPacakgeMigrationsStateKey] = packagesRequiringMigration;

                        return SbnDatabaseState.NeedsPackageMigration;
                    }
                }

                return SbnDatabaseState.Ok;
            }
            catch (Exception e)
            {
                // can connect to the database so cannot check the upgrade state... oops
                _logger.LogWarning(e, "Could not check the upgrade state.");

                // else it is bad enough that we want to throw
                Reason = RuntimeLevelReason.BootFailedCannotCheckUpgradeState;
                BootFailedException = new BootFailedException("Could not check the upgrade state.", e);
                throw BootFailedException;
            }
        }

        private bool DoesSbnRequireUpgrade(IReadOnlyDictionary<string, string> keyValues)
        {
            var upgrader = new Upgrader(new SbnPlan(_sbnVersion));
            var stateValueKey = upgrader.StateValueKey;

            _ = keyValues.TryGetValue(stateValueKey, out var value);

            CurrentMigrationState = value;
            FinalMigrationState = upgrader.Plan.FinalState;

            _logger.LogDebug("Final upgrade state is {FinalMigrationState}, database contains {DatabaseState}", FinalMigrationState, CurrentMigrationState ?? "<null>");

            return CurrentMigrationState != FinalMigrationState;
        }

        private bool TryDbConnect(ISbnDatabaseFactory databaseFactory)
        {
            // anything other than install wants a database - see if we can connect
            // (since this is an already existing database, assume localdb is ready)
            bool canConnect;
            var tries = _globalSettings.Value.InstallMissingDatabase ? 2 : 5;
            for (var i = 0; ;)
            {
                canConnect = databaseFactory.CanConnect;
                if (canConnect || ++i == tries)
                    break;
                _logger.LogDebug("Could not immediately connect to database, trying again.");
                Thread.Sleep(1000);
            }

            return canConnect;
        }

        private bool CanAutoInstallMissingDatabase(ISbnDatabaseFactory databaseFactory)
            => databaseFactory.ProviderName == Constants.DatabaseProviders.SqlCe ||
               databaseFactory.ConnectionString?.InvariantContains("(localdb)") == true;
    }
}
