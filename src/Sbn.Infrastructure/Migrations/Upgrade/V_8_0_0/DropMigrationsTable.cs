namespace Sbn.Cms.Infrastructure.Migrations.Upgrade.V_8_0_0
{
    public class DropMigrationsTable : MigrationBase
    {
        public DropMigrationsTable(IMigrationContext context)
            : base(context)
        { }

        protected override void Migrate()
        {
            if (TableExists("sbnMigration"))
                Delete.Table("sbnMigration").Do();
        }
    }
}
