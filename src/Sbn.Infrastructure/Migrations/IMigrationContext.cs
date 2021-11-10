using Microsoft.Extensions.Logging;
using Sbn.Cms.Infrastructure.Persistence;

namespace Sbn.Cms.Infrastructure.Migrations
{
    /// <summary>
    /// Provides context to migrations.
    /// </summary>
    public interface IMigrationContext
    {
        /// <summary>
        /// Gets the current migration plan
        /// </summary>
        MigrationPlan Plan { get; }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        ILogger<IMigrationContext> Logger { get; }

        /// <summary>
        /// Gets the database instance.
        /// </summary>
        ISbnDatabase Database { get; }

        /// <summary>
        /// Gets the Sql context.
        /// </summary>
        ISqlContext SqlContext { get; }

        /// <summary>
        /// Gets or sets the expression index.
        /// </summary>
        int Index { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether an expression is being built.
        /// </summary>
        bool BuildingExpression { get; set; }

        /// <summary>
        /// Adds a post-migration.
        /// </summary>
        void AddPostMigration<TMigration>()
            where TMigration : MigrationBase;
    }
}
