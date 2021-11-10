// Copyright (c) Sbn.
// See LICENSE for more details.

using Sbn.Cms.Infrastructure.Migrations;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Infrastructure.Migrations.Stubs
{
    public class SixZeroMigration2 : MigrationBase
    {
        public SixZeroMigration2(IMigrationContext context)
            : base(context)
        {
        }

        protected override void Migrate()
        {
            Alter.Table("sbnUser").AddColumn("secondEmail").AsString(255);
        }
    }
}
