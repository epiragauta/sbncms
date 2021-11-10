using System.Linq;
using Sbn.Cms.Core.Exceptions;

namespace Sbn.Cms.Infrastructure.Migrations.Upgrade.V_8_0_0.DataTypes
{
    class RenamingPreValueMigrator : DefaultPreValueMigrator
    {
        private readonly string[] _editors =
        {
            "Sbn.NoEdit"
        };

        public override bool CanMigrate(string editorAlias)
            => _editors.Contains(editorAlias);

        public override string GetNewAlias(string editorAlias)
        {
            switch (editorAlias)
            {
                case "Sbn.NoEdit":
                    return Cms.Core.Constants.PropertyEditors.Aliases.Label;
                default:
                    throw new PanicException($"The alias {editorAlias} is not supported");
            }
        }
    }
}
