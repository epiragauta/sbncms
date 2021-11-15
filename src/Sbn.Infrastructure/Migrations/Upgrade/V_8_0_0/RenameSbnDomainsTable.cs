namespace Sbn.Cms.Infrastructure.Migrations.Upgrade.V_8_0_0
{
    public class RenameSbnDomainsTable : MigrationBase
    {
        public RenameSbnDomainsTable(IMigrationContext context)
            : base(context)
        { }

        protected override void Migrate()
        {
            Rename.Table("sbnDomains").To(Cms.Core.Constants.DatabaseSchema.Tables.Domain).Do();
        }
    }
}
