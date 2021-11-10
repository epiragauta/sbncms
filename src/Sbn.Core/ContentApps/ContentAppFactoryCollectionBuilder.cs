using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sbn.Cms.Core.Composing;
using Sbn.Cms.Core.IO;
using Sbn.Cms.Core.Manifest;
using Sbn.Cms.Core.Models.ContentEditing;
using Sbn.Cms.Core.Security;

namespace Sbn.Cms.Core.ContentApps
{
    public class ContentAppFactoryCollectionBuilder : OrderedCollectionBuilderBase<ContentAppFactoryCollectionBuilder, ContentAppFactoryCollection, IContentAppFactory>
    {
        protected override ContentAppFactoryCollectionBuilder This => this;

        // need to inject dependencies in the collection, so override creation
        public override ContentAppFactoryCollection CreateCollection(IServiceProvider factory)
        {
            // get the logger factory just-in-time - see note below for manifest parser
            var loggerFactory = factory.GetRequiredService<ILoggerFactory>();
            var backOfficeSecurityAccessor = factory.GetRequiredService<IBackOfficeSecurityAccessor>();
            return new ContentAppFactoryCollection(
                () => CreateItems(factory),
                loggerFactory.CreateLogger<ContentAppFactoryCollection>(), backOfficeSecurityAccessor);
        }

        protected override IEnumerable<IContentAppFactory> CreateItems(IServiceProvider factory)
        {
            // get the manifest parser just-in-time - injecting it in the ctor would mean that
            // simply getting the builder in order to configure the collection, would require
            // its dependencies too, and that can create cycles or other oddities
            var manifestParser = factory.GetRequiredService<IManifestParser>();
            var ioHelper = factory.GetRequiredService<IIOHelper>();
            return base.CreateItems(factory).Concat(manifestParser.CombinedManifest.ContentApps.Select(x => new ManifestContentAppFactory(x, ioHelper)));
        }
    }
}
