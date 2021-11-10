using System;
using System.IO;
using System.Linq;
using System.Web.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Sbn.Cms.Core.Cache;
using Sbn.Cms.Core.Composing;
using Sbn.Cms.Core.Hosting;
using Sbn.Cms.Core.IO;
using Sbn.Cms.Core.Logging;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Models.PublishedContent;
using Sbn.Cms.Core.PropertyEditors;
using Sbn.Cms.Core.PublishedCache;
using Sbn.Cms.Core.Security;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Strings;
using Sbn.Cms.Core.Web;
using Sbn.Cms.Infrastructure.Serialization;
using Sbn.Cms.Tests.Common;
using Sbn.Extensions;
using Sbn.Tests.TestHelpers;
using Sbn.Web;

namespace Sbn.Tests.PublishedContent
{
    public abstract class PublishedContentSnapshotTestBase : PublishedContentTestBase
    {
        // read http://stackoverflow.com/questions/7713326/extension-method-that-works-on-ienumerablet-and-iqueryablet
        // and http://msmvps.com/blogs/jon_skeet/archive/2010/10/28/overloading-and-generic-constraints.aspx
        // and http://blogs.msdn.com/b/ericlippert/archive/2009/12/10/constraints-are-not-part-of-the-signature.aspx

        public override void SetUp()
        {
            base.SetUp();

            var sbnContext = GetSbnContext();
            Sbn.Web.Composing.Current.SbnContextAccessor.SbnContext = sbnContext;
        }

        protected override void Compose()
        {
            base.Compose();

            Builder.Services.AddUnique<IPublishedModelFactory>(f => new PublishedModelFactory(f.GetRequiredService<TypeLoader>().GetTypes<PublishedContentModel>(), f.GetRequiredService<IPublishedValueFallback>()));
        }

        protected override TypeLoader CreateTypeLoader(IIOHelper ioHelper, ITypeFinder typeFinder, IAppPolicyCache runtimeCache, ILogger<TypeLoader> logger, IProfilingLogger profilingLogger ,  IHostingEnvironment hostingEnvironment)
        {
            var baseLoader = base.CreateTypeLoader(ioHelper, typeFinder, runtimeCache, logger, profilingLogger , hostingEnvironment);

            return new TypeLoader(typeFinder, runtimeCache, new DirectoryInfo(hostingEnvironment.LocalTempPath), logger, profilingLogger , false,
                // this is so the model factory looks into the test assembly
                baseLoader.AssembliesToScan
                    .Union(new[] {typeof(PublishedContentMoreTests).Assembly})
                    .ToList());
        }

        private ISbnContext GetSbnContext()
        {
            RouteData routeData = null;

            var publishedSnapshot = CreatePublishedSnapshot();

            var publishedSnapshotService = new Mock<IPublishedSnapshotService>();
            publishedSnapshotService.Setup(x => x.CreatePublishedSnapshot(It.IsAny<string>())).Returns(publishedSnapshot);

            var globalSettings = TestObjects.GetGlobalSettings();

            var httpContext = GetHttpContextFactory("http://sbn.local/", routeData).HttpContext;

            var httpContextAccessor = TestHelper.GetHttpContextAccessor(httpContext);
            var sbnContext = new SbnContext(
                httpContextAccessor,
                publishedSnapshotService.Object,
                Mock.Of<IBackOfficeSecurity>(),
                globalSettings,
                HostingEnvironment,
                new TestVariationContextAccessor(),
                UriUtility,
                new AspNetCookieManager(httpContextAccessor));

            return sbnContext;
        }

        private SolidPublishedSnapshot CreatePublishedSnapshot()
        {
            var serializer = new ConfigurationEditorJsonSerializer();
            var dataTypeService = new TestObjects.TestDataTypeService(
                new DataType(new VoidEditor(DataValueEditorFactory), serializer) { Id = 1 });

            var factory = new PublishedContentTypeFactory(Mock.Of<IPublishedModelFactory>(), new PropertyValueConverterCollection(Array.Empty<IPropertyValueConverter>()), dataTypeService);
            var caches = new SolidPublishedSnapshot();
            var cache = caches.InnerContentCache;
            PopulateCache(factory, cache);
            return caches;
        }

        internal abstract void PopulateCache(PublishedContentTypeFactory factory, SolidPublishedContentCache cache);
    }
}
