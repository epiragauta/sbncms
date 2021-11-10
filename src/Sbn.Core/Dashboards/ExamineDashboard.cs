using System;
using Sbn.Cms.Core.Composing;

namespace Sbn.Cms.Core.Dashboards
{
    [Weight(20)]
    public class ExamineDashboard : IDashboard
    {
        public string Alias => "settingsExamine";

        public string[] Sections => new [] { "settings" };

        public string View => "views/dashboard/settings/examinemanagement.html";

        public IAccessRule[] AccessRules => Array.Empty<IAccessRule>();
    }


}
