using Sbn.Cms.Core.Composing;

namespace Sbn.Cms.Core.Packaging
{
    public class PackageMigrationPlanCollectionBuilder : LazyCollectionBuilderBase<PackageMigrationPlanCollectionBuilder, PackageMigrationPlanCollection, PackageMigrationPlan>
    {
        protected override PackageMigrationPlanCollectionBuilder This => this;
    }
}
