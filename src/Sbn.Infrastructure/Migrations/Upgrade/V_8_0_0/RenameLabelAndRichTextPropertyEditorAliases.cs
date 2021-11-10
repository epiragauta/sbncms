using System.Collections.Generic;
using Sbn.Cms.Infrastructure.Persistence.Dtos;
using Sbn.Extensions;

namespace Sbn.Cms.Infrastructure.Migrations.Upgrade.V_8_0_0
{
    public class RenameLabelAndRichTextPropertyEditorAliases : MigrationBase
    {
        public RenameLabelAndRichTextPropertyEditorAliases(IMigrationContext context)
            : base(context)
        {
        }

        protected override void Migrate()
        {
            MigratePropertyEditorAlias("Sbn.TinyMCEv3", Cms.Core.Constants.PropertyEditors.Aliases.TinyMce);
            MigratePropertyEditorAlias("Sbn.NoEdit", Cms.Core.Constants.PropertyEditors.Aliases.Label);
        }

        private void MigratePropertyEditorAlias(string oldAlias, string newAlias)
        {
            var dataTypes = GetDataTypes(oldAlias);

            foreach (var dataType in dataTypes)
            {
                dataType.EditorAlias = newAlias;
                Database.Update(dataType);
            }
        }

        private List<DataTypeDto> GetDataTypes(string editorAlias)
        {
            var dataTypes = Database.Fetch<DataTypeDto>(Sql()
                .Select<DataTypeDto>()
                .From<DataTypeDto>()
                .Where<DataTypeDto>(x => x.EditorAlias == editorAlias));
            return dataTypes;
        }

    }
}
