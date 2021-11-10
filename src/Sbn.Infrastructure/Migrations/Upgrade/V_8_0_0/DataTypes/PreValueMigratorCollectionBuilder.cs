using Sbn.Cms.Core.Composing;

namespace Sbn.Cms.Infrastructure.Migrations.Upgrade.V_8_0_0.DataTypes
{
    public class PreValueMigratorCollectionBuilder : OrderedCollectionBuilderBase<PreValueMigratorCollectionBuilder, PreValueMigratorCollection, IPreValueMigrator>
    {
        protected override PreValueMigratorCollectionBuilder This => this;
    }
}
