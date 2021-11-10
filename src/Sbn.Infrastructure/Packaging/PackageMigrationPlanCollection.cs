using System;
using System.Collections.Generic;
using Sbn.Cms.Core.Composing;

namespace Sbn.Cms.Core.Packaging
{
    /// <summary>
    /// A collection of <see cref="PackageMigrationPlan"/>
    /// </summary>
    public class PackageMigrationPlanCollection : BuilderCollectionBase<PackageMigrationPlan>
    {
        public PackageMigrationPlanCollection(Func<IEnumerable<PackageMigrationPlan>> items) : base(items)
        {
        }
    }
}
