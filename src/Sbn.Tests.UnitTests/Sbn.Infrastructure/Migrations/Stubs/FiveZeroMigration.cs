// Copyright (c) Sbn.
// See LICENSE for more details.

using Sbn.Cms.Infrastructure.Migrations;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Infrastructure.Migrations.Stubs
{
    public class FiveZeroMigration : MigrationBase
    {
        public FiveZeroMigration(IMigrationContext context)
            : base(context)
        {
        }

        protected override void Migrate()
        {
        }
    }
}
