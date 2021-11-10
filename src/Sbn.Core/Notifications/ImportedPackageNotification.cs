using Sbn.Cms.Core.Models.Packaging;
using Sbn.Cms.Core.Packaging;

namespace Sbn.Cms.Core.Notifications
{
    public class ImportedPackageNotification : StatefulNotification
    {
        public ImportedPackageNotification(InstallationSummary installationSummary)
        {
            InstallationSummary = installationSummary;
        }

        public InstallationSummary InstallationSummary { get; }
    }
}
