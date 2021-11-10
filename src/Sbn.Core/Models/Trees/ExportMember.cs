using Sbn.Cms.Core.Services;

namespace Sbn.Cms.Core.Models.Trees
{
    /// <summary>
    /// Represents the export member menu item
    /// </summary>
    public sealed class ExportMember : ActionMenuItem
    {
        public override string AngularServiceName => "sbnMenuActions";

        public ExportMember(ILocalizedTextService textService) : base("export", textService)
        {
            Icon = "download-alt";
        }
    }
}
