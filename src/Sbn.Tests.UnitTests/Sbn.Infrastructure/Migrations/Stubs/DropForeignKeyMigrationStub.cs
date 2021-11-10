// Copyright (c) Sbn.
// See LICENSE for more details.

using Sbn.Cms.Infrastructure.Migrations;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Infrastructure.Migrations.Stubs
{
    public class DropForeignKeyMigrationStub : MigrationBase
    {
        public DropForeignKeyMigrationStub(IMigrationContext context)
            : base(context)
        {
        }

        protected override void Migrate()
        {
            Delete.ForeignKey().FromTable("sbnUser2app").ForeignColumn("user").ToTable("sbnUser").PrimaryColumn("id").Do();
        }
    }
}
