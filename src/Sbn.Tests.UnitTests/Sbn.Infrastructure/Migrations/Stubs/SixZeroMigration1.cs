// Copyright (c) Sbn.
// See LICENSE for more details.

using Sbn.Cms.Infrastructure.Migrations;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Infrastructure.Migrations.Stubs
{
    public class SixZeroMigration1 : MigrationBase
    {
        public SixZeroMigration1(IMigrationContext context)
            : base(context)
        {
        }

        protected override void Migrate()
        {
            Alter.Table("sbnUser").AddColumn("secret").AsString(255);
        }
    }
}
