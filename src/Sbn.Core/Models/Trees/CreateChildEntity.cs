using Sbn.Cms.Core.Actions;
using Sbn.Cms.Core.Services;

namespace Sbn.Cms.Core.Models.Trees
{
    /// <summary>
    /// Represents the refresh node menu item
    /// </summary>
    public sealed class CreateChildEntity : ActionMenuItem
    {
        public override string AngularServiceName => "sbnMenuActions";

        public CreateChildEntity(string name, bool separatorBefore = false)
            : base(ActionNew.ActionAlias, name)
        {
            Icon = "add"; Name = name;
            SeparatorBefore = separatorBefore;
        }

        public CreateChildEntity(ILocalizedTextService textService, bool separatorBefore = false)
            : base(ActionNew.ActionAlias, textService)
        {
            Icon = "add";
            SeparatorBefore = separatorBefore;
        }
    }
}
