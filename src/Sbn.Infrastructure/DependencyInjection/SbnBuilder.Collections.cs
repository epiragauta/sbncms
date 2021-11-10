using Sbn.Cms.Core.DependencyInjection;
using Sbn.Cms.Core.Packaging;
using Sbn.Cms.Infrastructure.Persistence;
using Sbn.Cms.Infrastructure.Persistence.Mappers;

namespace Sbn.Extensions
{
    /// <summary>
    /// Provides extension methods to the <see cref="ISbnBuilder"/> class.
    /// </summary>
    public static partial class SbnBuilderExtensions
    {
        /// <summary>
        /// Gets the mappers collection builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static MapperCollectionBuilder Mappers(this ISbnBuilder builder)
            => builder.WithCollectionBuilder<MapperCollectionBuilder>();

        public static NPocoMapperCollectionBuilder NPocoMappers(this ISbnBuilder builder)
            => builder.WithCollectionBuilder<NPocoMapperCollectionBuilder>();


        /// <summary>
        /// Gets the package migration plans collection builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static PackageMigrationPlanCollectionBuilder PackageMigrationPlans(this ISbnBuilder builder)
            => builder.WithCollectionBuilder<PackageMigrationPlanCollectionBuilder>();

    }
}
