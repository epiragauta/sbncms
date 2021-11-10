using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sbn.Cms.Core;
using Sbn.Cms.Core.Install;
using Sbn.Cms.Core.Install.Models;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Infrastructure.Migrations.Install;

namespace Sbn.Cms.Infrastructure.Install.InstallSteps
{
    [InstallSetupStep(InstallationType.NewInstall | InstallationType.Upgrade, "DatabaseInstall", 11, "")]
    public class DatabaseInstallStep : InstallSetupStep<object>
    {
        private readonly IRuntimeState _runtime;
        private readonly DatabaseBuilder _databaseBuilder;

        public DatabaseInstallStep(IRuntimeState runtime, DatabaseBuilder databaseBuilder)
        {
            _runtime = runtime;
            _databaseBuilder = databaseBuilder;
        }

        public override Task<InstallSetupResult> ExecuteAsync(object model)
        {
            if (_runtime.Level == RuntimeLevel.Run)
                throw new Exception("Sbn is already configured!");

            if (_runtime.Reason == RuntimeLevelReason.InstallMissingDatabase)
            {
                _databaseBuilder.CreateDatabase();
            }

            var result = _databaseBuilder.CreateSchemaAndData();

            if (result.Success == false)
            {
                throw new InstallException("The database failed to install. ERROR: " + result.Message);
            }

            if (result.RequiresUpgrade == false)
            {
                return Task.FromResult<InstallSetupResult>(null);
            }

            // Upgrade is required, so set the flag for the next step
            return Task.FromResult(new InstallSetupResult(new Dictionary<string, object>
            {
                { "upgrade", true}
            }));
        }

        public override bool RequiresExecution(object model) => true;
    }
}
