using System;
using Sbn.Cms.Core.Composing;

namespace Sbn.Cms.Core.Dashboards
{
    [Weight(40)]
    public class ModelsBuilderDashboard : IDashboard
    {
        public string Alias => "settingsModelsBuilder";

        public string[] Sections => new [] { "settings" };

        public string View => "views/dashboard/settings/modelsbuildermanagement.html";

        public IAccessRule[] AccessRules => Array.Empty<IAccessRule>();
    }
}
