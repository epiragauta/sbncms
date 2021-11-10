// Copyright (c) Sbn.
// See LICENSE for more details.

using Sbn.Cms.Infrastructure.Migrations;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Infrastructure.Migrations.Stubs
{
    public class FourElevenMigration : MigrationBase
    {
        public FourElevenMigration(IMigrationContext context)
            : base(context)
        {
        }

        protected override void Migrate()
        {
            Alter.Table("sbnUser").AddColumn("companyPhone").AsString(255);
        }
    }
}
