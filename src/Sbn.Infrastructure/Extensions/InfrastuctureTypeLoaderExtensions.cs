using System;
using System.Collections.Generic;
using Sbn.Cms.Core.Composing;
using Sbn.Cms.Core.Packaging;

namespace Sbn.Extensions
{
    public static class InfrastuctureTypeLoaderExtensions
    {
        /// <summary>
        /// Gets all types implementing <see cref="PackageMigrationPlan"/>
        /// </summary>
        /// <param name="mgr"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetPackageMigrationPlans(this TypeLoader mgr) => mgr.GetTypes<PackageMigrationPlan>();

    }
}
