using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Sbn.Cms.Core.Migrations;
using Sbn.Cms.Infrastructure.Persistence;

namespace Sbn.Cms.Infrastructure.Migrations
{
    /// <summary>
    /// Implements <see cref="IMigrationContext"/>.
    /// </summary>
    internal class MigrationContext : IMigrationContext
    {
        private readonly List<Type> _postMigrations = new List<Type>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MigrationContext"/> class.
        /// </summary>
        public MigrationContext(MigrationPlan plan, ISbnDatabase database, ILogger<MigrationContext> logger)
        {
            Plan = plan;
            Database = database ?? throw new ArgumentNullException(nameof(database));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _postMigrations.AddRange(plan.PostMigrationTypes);
        }

        /// <inheritdoc />
        public ILogger<IMigrationContext> Logger { get; }

        public MigrationPlan Plan { get; }

        /// <inheritdoc />
        public ISbnDatabase Database { get; }

        /// <inheritdoc />
        public ISqlContext SqlContext => Database.SqlContext;

        /// <inheritdoc />
        public int Index { get; set; }

        /// <inheritdoc />
        public bool BuildingExpression { get; set; }

        // this is only internally exposed
        public IReadOnlyList<Type> PostMigrations => _postMigrations;

        /// <inheritdoc />
        public void AddPostMigration<TMigration>()
            where TMigration : MigrationBase
        {
            // just adding - will be de-duplicated when executing
            _postMigrations.Add(typeof(TMigration));
        }
    }
}
