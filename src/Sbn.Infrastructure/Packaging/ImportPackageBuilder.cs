using System;
using System.Xml.Linq;
using Microsoft.Extensions.Options;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.IO;
using Sbn.Cms.Core.PropertyEditors;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Strings;
using Sbn.Cms.Infrastructure.Migrations;
using Sbn.Cms.Infrastructure.Migrations.Expressions;
using Sbn.Cms.Infrastructure.Migrations.Expressions.Common;

namespace Sbn.Cms.Infrastructure.Packaging
{
    internal class ImportPackageBuilder : ExpressionBuilderBase<ImportPackageBuilderExpression>, IImportPackageBuilder, IExecutableBuilder
    {
        public ImportPackageBuilder(
            IPackagingService packagingService,
            IMediaService mediaService,
            MediaFileManager mediaFileManager,
            MediaUrlGeneratorCollection mediaUrlGenerators,
            IShortStringHelper shortStringHelper,
            IContentTypeBaseServiceProvider contentTypeBaseServiceProvider,
            IMigrationContext context,
            IOptions<PackageMigrationSettings> options)
            : base(new ImportPackageBuilderExpression(
                packagingService,
                mediaService,
                mediaFileManager,
                mediaUrlGenerators,
                shortStringHelper,
                contentTypeBaseServiceProvider,
                context,
                options))
        {
        }

        public void Do() => Expression.Execute();

        public IExecutableBuilder FromEmbeddedResource<TPackageMigration>()
            where TPackageMigration : PackageMigrationBase
        {
            Expression.EmbeddedResourceMigrationType = typeof(TPackageMigration);
            return this;
        }

        public IExecutableBuilder FromEmbeddedResource(Type packageMigrationType)
        {
            Expression.EmbeddedResourceMigrationType = packageMigrationType;
            return this;
        }

        public IExecutableBuilder FromXmlDataManifest(XDocument packageDataManifest)
        {
            Expression.PackageDataManifest = packageDataManifest;
            return this;
        }
    }
}
