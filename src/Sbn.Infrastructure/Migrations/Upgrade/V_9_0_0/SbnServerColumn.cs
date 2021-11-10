using Sbn.Cms.Infrastructure.Persistence.Dtos;

namespace Sbn.Cms.Infrastructure.Migrations.Upgrade.V_9_0_0
{
    public class SbnServerColumn : MigrationBase
    {
        public SbnServerColumn(IMigrationContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Adds new columns to members table
        /// </summary>
        protected override void Migrate()
        {
            ReplaceColumn<ServerRegistrationDto>(Cms.Core.Constants.DatabaseSchema.Tables.Server, "isMaster", "isSchedulingPublisher");
        }
    }
}
