using System;
using System.Collections.Generic;
using Sbn.Cms.Core.Composing;

namespace Sbn.Cms.Core.Manifest
{
    /// <summary>
    /// Contains the manifest filters.
    /// </summary>
    public class ManifestFilterCollection : BuilderCollectionBase<IManifestFilter>
    {
        public ManifestFilterCollection(Func<IEnumerable<IManifestFilter>> items) : base(items)
        {
        }

        /// <summary>
        /// Filters package manifests.
        /// </summary>
        /// <param name="manifests">The package manifests.</param>
        public void Filter(List<PackageManifest> manifests)
        {
            foreach (var filter in this)
                filter.Filter(manifests);
        }
    }
}
