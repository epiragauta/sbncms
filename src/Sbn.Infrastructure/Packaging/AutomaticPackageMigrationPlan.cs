using System;
using Sbn.Cms.Core.IO;
using Sbn.Cms.Core.Packaging;
using Sbn.Cms.Core.PropertyEditors;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Strings;
using Sbn.Cms.Infrastructure.Migrations;
using Sbn.Extensions;

namespace Sbn.Cms.Infrastructure.Packaging
{
    /// <summary>
    /// Used to automatically indicate that a package has an embedded package data manifest that needs to be installed
    /// </summary>
    public abstract class AutomaticPackageMigrationPlan : PackageMigrationPlan
    {
        protected AutomaticPackageMigrationPlan(string packageName)
            : this(packageName, packageName)
        { }

        protected AutomaticPackageMigrationPlan(string packageName, string planName)
            : base(packageName, planName)
        { }

        protected sealed override void DefinePlan()
        {
            // calculate the final state based on the hash value of the embedded resource
            Type planType = GetType();
            var hash = PackageMigrationResource.GetEmbeddedPackageDataManifestHash(planType);

            var finalId = hash.ToGuid();
            To<MigrateToPackageData>(finalId);
        }

        private class MigrateToPackageData : PackageMigrationBase
        {
            public MigrateToPackageData(IPackagingService packagingService, IMediaService mediaService, MediaFileManager mediaFileManager, MediaUrlGeneratorCollection mediaUrlGenerators, IShortStringHelper shortStringHelper, IContentTypeBaseServiceProvider contentTypeBaseServiceProvider, IMigrationContext context) : base(packagingService, mediaService, mediaFileManager, mediaUrlGenerators, shortStringHelper, contentTypeBaseServiceProvider, context)
            {
            }

            protected override void Migrate()
            {
                var plan = (AutomaticPackageMigrationPlan)Context.Plan;

                ImportPackage.FromEmbeddedResource(plan.GetType()).Do();
            }
        }
    }
}
