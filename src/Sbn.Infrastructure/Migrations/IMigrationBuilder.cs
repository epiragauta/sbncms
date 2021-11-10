using System;

namespace Sbn.Cms.Infrastructure.Migrations
{
    public interface IMigrationBuilder
    {
        MigrationBase Build(Type migrationType, IMigrationContext context);
    }
}
