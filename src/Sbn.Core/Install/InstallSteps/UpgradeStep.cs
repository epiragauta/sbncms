using System;
using System.Threading.Tasks;
using Sbn.Cms.Core.Configuration;
using Sbn.Cms.Core.Install.Models;
using Sbn.Cms.Core.Semver;
using Sbn.Cms.Core.Services;
using Sbn.Extensions;
namespace Sbn.Cms.Core.Install.InstallSteps
{
    /// <summary>
    /// This step is purely here to show the button to commence the upgrade
    /// </summary>
    [InstallSetupStep(InstallationType.Upgrade, "Upgrade", "upgrade", 1, "Upgrading Sbn to the latest and greatest version.")]
    public class UpgradeStep : InstallSetupStep<object>
    {
        public override bool RequiresExecution(object model) => true;
        private readonly ISbnVersion _sbnVersion;
        private readonly IRuntimeState _runtimeState;

        public UpgradeStep(ISbnVersion sbnVersion, IRuntimeState runtimeState)
        {
            _sbnVersion = sbnVersion;
            _runtimeState = runtimeState;
        }

        public override Task<InstallSetupResult> ExecuteAsync(object model) => Task.FromResult<InstallSetupResult>(null);

        public override object ViewModel
        {
            get
            {
                string FormatGuidState(string value)
                {
                    if (string.IsNullOrWhiteSpace(value)) value = "unknown";
                    else if (Guid.TryParse(value, out var currentStateGuid))
                        value = currentStateGuid.ToString("N").Substring(0, 8);
                    return value;
                }

                var currentState = FormatGuidState(_runtimeState.CurrentMigrationState);
                var newState = FormatGuidState(_runtimeState.FinalMigrationState);
                var newVersion = _sbnVersion.SemanticVersion.ToSemanticStringWithoutBuild();
                var oldVersion = new SemVersion(_sbnVersion.SemanticVersion.Major, 0, 0).ToString(); //TODO can we find the old version somehow? e.g. from current state

                var reportUrl = $"https://our.sbn.com/contribute/releases/compare?from={oldVersion}&to={newVersion}&notes=1";

                return new { oldVersion, newVersion, currentState, newState, reportUrl };
            }
        }
    }
}
