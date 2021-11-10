using System;
using System.Xml.Linq;
using Sbn.Cms.Infrastructure.Migrations.Expressions;
using Sbn.Cms.Infrastructure.Migrations.Expressions.Common;

namespace Sbn.Cms.Infrastructure.Packaging
{
    public interface IImportPackageBuilder : IFluentBuilder
    {
        IExecutableBuilder FromEmbeddedResource<TPackageMigration>()
            where TPackageMigration : PackageMigrationBase;

        IExecutableBuilder FromEmbeddedResource(Type packageMigrationType);

        IExecutableBuilder FromXmlDataManifest(XDocument packageDataManifest);
    }
}
