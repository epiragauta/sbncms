using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Sbn.Cms.Core.IO;
using Sbn.Cms.Core.Media;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Models.PublishedContent;
using Sbn.Cms.Core.PropertyEditors;
using Sbn.Cms.Core.PropertyEditors.ValueConverters;
using Sbn.Cms.Core.Routing;
using Sbn.Cms.Core.Security;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Strings;
using Sbn.Cms.Core.Templates;
using Sbn.Cms.Core.Web;
using Sbn.Cms.Infrastructure.Serialization;
using Sbn.Tests.TestHelpers;

namespace Sbn.Tests.PublishedContent
{
    /// <summary>
    /// Abstract base class for tests for published content and published media
    /// </summary>
    public abstract class PublishedContentTestBase : BaseWebTest
    {
        protected override void Compose()
        {
            base.Compose();

            // FIXME: what about the if (PropertyValueConvertersResolver.HasCurrent == false) ??
            // can we risk double - registering and then, what happens?

            Builder.WithCollectionBuilder<PropertyValueConverterCollectionBuilder>()
                .Clear()
                .Append<DatePickerValueConverter>()
                .Append<SimpleTinyMceValueConverter>()
                .Append<YesNoValueConverter>();
        }

        protected override void Initialize()
        {
            base.Initialize();

            var converters = Factory.GetRequiredService<PropertyValueConverterCollection>();
            var sbnContextAccessor = Mock.Of<ISbnContextAccessor>();
            var publishedUrlProvider = Mock.Of<IPublishedUrlProvider>();
            var loggerFactory = NullLoggerFactory.Instance;
            var serializer = new ConfigurationEditorJsonSerializer();

            var imageSourceParser = new HtmlImageSourceParser(publishedUrlProvider);
            var mediaFileManager = new MediaFileManager(Mock.Of<IFileSystem>(), Mock.Of<IMediaPathScheme>(),
                loggerFactory.CreateLogger<MediaFileManager>(), Mock.Of<IShortStringHelper>());
            var pastedImages = new RichTextEditorPastedImages(sbnContextAccessor, loggerFactory.CreateLogger<RichTextEditorPastedImages>(), HostingEnvironment,  Mock.Of<IMediaService>(), Mock.Of<IContentTypeBaseServiceProvider>(), mediaFileManager, ShortStringHelper, publishedUrlProvider, serializer);
            var localLinkParser = new HtmlLocalLinkParser(sbnContextAccessor, publishedUrlProvider);
            var dataTypeService = new TestObjects.TestDataTypeService(
                new DataType(new RichTextPropertyEditor(
                        DataValueEditorFactory,
                    Mock.Of<IBackOfficeSecurityAccessor>(),
                        imageSourceParser,
                    localLinkParser,
                    pastedImages,
                        IOHelper,
                        Mock.Of<IImageUrlGenerator>()),
                    serializer) { Id = 1 });


            var publishedContentTypeFactory = new PublishedContentTypeFactory(Mock.Of<IPublishedModelFactory>(), converters, dataTypeService);

            IEnumerable<IPublishedPropertyType> CreatePropertyTypes(IPublishedContentType contentType)
            {
                yield return publishedContentTypeFactory.CreatePropertyType(contentType, "content", 1);
            }

            var type = new AutoPublishedContentType(Guid.NewGuid(), 0, "anything", CreatePropertyTypes);
            ContentTypesCache.GetPublishedContentTypeByAlias = alias => type;

            var sbnContext = GetSbnContext("/test");
            Sbn.Web.Composing.Current.SbnContextAccessor.SbnContext = sbnContext;
        }
    }
}
