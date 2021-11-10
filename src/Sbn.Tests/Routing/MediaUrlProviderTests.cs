using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.IO;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Models.PublishedContent;
using Sbn.Cms.Core.PropertyEditors;
using Sbn.Cms.Core.PropertyEditors.ValueConverters;
using Sbn.Cms.Core.Routing;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Strings;
using Sbn.Cms.Core.Web;
using Sbn.Cms.Tests.Common;
using Sbn.Cms.Tests.Common.Testing;
using Sbn.Tests.PublishedContent;
using Sbn.Tests.TestHelpers;
using Constants = Sbn.Cms.Core.Constants;

namespace Sbn.Tests.Routing
{
    [TestFixture]
    [SbnTest(Database = SbnTestOptions.Database.NewSchemaPerFixture)]
    public class MediaUrlProviderTests : BaseWebTest
    {
        private DefaultMediaUrlProvider _mediaUrlProvider;

        public override void SetUp()
        {
            base.SetUp();

            var loggerFactory = NullLoggerFactory.Instance;
            var mediaFileManager = new MediaFileManager(Mock.Of<IFileSystem>(), Mock.Of<IMediaPathScheme>(),
                loggerFactory.CreateLogger<MediaFileManager>(), Mock.Of<IShortStringHelper>());
            var contentSettings = new ContentSettings();
            var dataTypeService = Mock.Of<IDataTypeService>();
            var propertyEditors = new MediaUrlGeneratorCollection(new IMediaUrlGenerator[]
            {
                new FileUploadPropertyEditor(DataValueEditorFactory, mediaFileManager, Microsoft.Extensions.Options.Options.Create(contentSettings), dataTypeService, LocalizationService, LocalizedTextService,  UploadAutoFillProperties, ContentService),
                new ImageCropperPropertyEditor(DataValueEditorFactory, loggerFactory, mediaFileManager, Microsoft.Extensions.Options.Options.Create(contentSettings), dataTypeService, IOHelper, UploadAutoFillProperties, ContentService),
            });
            _mediaUrlProvider = new DefaultMediaUrlProvider(propertyEditors, UriUtility);
        }

        public override void TearDown()
        {
            base.TearDown();

            _mediaUrlProvider = null;
        }

        [Test]
        public void Get_Media_Url_Resolves_Url_From_Upload_Property_Editor()
        {
            const string expected = "/media/rfeiw584/test.jpg";

            var sbnContext = GetSbnContext("/");
            var publishedContent = CreatePublishedContent(Constants.PropertyEditors.Aliases.UploadField, expected, null);

            var resolvedUrl = GetPublishedUrlProvider(sbnContext).GetMediaUrl(publishedContent, UrlMode.Auto);

            Assert.AreEqual(expected, resolvedUrl);
        }

        [Test]
        public void Get_Media_Url_Resolves_Url_From_Image_Cropper_Property_Editor()
        {
            const string expected = "/media/rfeiw584/test.jpg";

            var configuration = new ImageCropperConfiguration();
            var imageCropperValue = JsonConvert.SerializeObject(new ImageCropperValue
            {
                Src = expected
            });

            var sbnContext = GetSbnContext("/");
            var publishedContent = CreatePublishedContent(Constants.PropertyEditors.Aliases.ImageCropper, imageCropperValue, configuration);

            var resolvedUrl = GetPublishedUrlProvider(sbnContext).GetMediaUrl(publishedContent, UrlMode.Auto);

            Assert.AreEqual(expected, resolvedUrl);
        }

        [Test]
        public void Get_Media_Url_Can_Resolve_Absolute_Url()
        {
            const string mediaUrl = "/media/rfeiw584/test.jpg";
            var expected = $"http://localhost{mediaUrl}";

            var sbnContext = GetSbnContext("http://localhost");
            var publishedContent = CreatePublishedContent(Constants.PropertyEditors.Aliases.UploadField, mediaUrl, null);

            var resolvedUrl = GetPublishedUrlProvider(sbnContext).GetMediaUrl(publishedContent, UrlMode.Absolute);

            Assert.AreEqual(expected, resolvedUrl);
        }

        [Test]
        public void Get_Media_Url_Returns_Absolute_Url_If_Stored_Url_Is_Absolute()
        {
            const string expected = "http://localhost/media/rfeiw584/test.jpg";

            var sbnContext = GetSbnContext("http://localhost");
            var publishedContent = CreatePublishedContent(Constants.PropertyEditors.Aliases.UploadField, expected, null);

            var resolvedUrl = GetPublishedUrlProvider(sbnContext).GetMediaUrl(publishedContent, UrlMode.Relative);

            Assert.AreEqual(expected, resolvedUrl);
        }

        [Test]
        public void Get_Media_Url_Returns_Empty_String_When_PropertyType_Is_Not_Supported()
        {
            var sbnContext = GetSbnContext("/");
            var publishedContent = CreatePublishedContent(Constants.PropertyEditors.Aliases.Boolean, "0", null);

            var resolvedUrl = GetPublishedUrlProvider(sbnContext).GetMediaUrl(publishedContent, UrlMode.Absolute, propertyAlias: "test");

            Assert.AreEqual(string.Empty, resolvedUrl);
        }

        [Test]
        public void Get_Media_Url_Can_Resolve_Variant_Property_Url()
        {
            var sbnContext = GetSbnContext("http://localhost");

            var sbnFilePropertyType = CreatePropertyType(Constants.PropertyEditors.Aliases.UploadField, null, ContentVariation.Culture);

            const string enMediaUrl = "/media/rfeiw584/en.jpg";
            const string daMediaUrl = "/media/uf8ewud2/da.jpg";

            var property = new SolidPublishedPropertyWithLanguageVariants
            {
                Alias = "sbnFile",
                PropertyType = sbnFilePropertyType,
            };

            property.SetSourceValue("en", enMediaUrl, true);
            property.SetSourceValue("da", daMediaUrl);

            var contentType = new PublishedContentType(Guid.NewGuid(), 666, "alias", PublishedItemType.Content, Enumerable.Empty<string>(), new [] { sbnFilePropertyType }, ContentVariation.Culture);
            var publishedContent = new SolidPublishedContent(contentType) {Properties = new[] {property}};

            var resolvedUrl = GetPublishedUrlProvider(sbnContext).GetMediaUrl(publishedContent, UrlMode.Auto, "da");
            Assert.AreEqual(daMediaUrl, resolvedUrl);
        }

        private IPublishedUrlProvider GetPublishedUrlProvider(ISbnContext sbnContext)
        {
            var webRoutingSettings = new WebRoutingSettings();
            return new UrlProvider(
                new TestSbnContextAccessor(sbnContext),
                Microsoft.Extensions.Options.Options.Create(webRoutingSettings),
                new UrlProviderCollection(Enumerable.Empty<IUrlProvider>()),
                new MediaUrlProviderCollection(new []{_mediaUrlProvider}),
                Mock.Of<IVariationContextAccessor>()
            );
        }

        private static IPublishedContent CreatePublishedContent(string propertyEditorAlias, string propertyValue, object dataTypeConfiguration)
        {
            var sbnFilePropertyType = CreatePropertyType(propertyEditorAlias, dataTypeConfiguration, ContentVariation.Nothing);

            var contentType = new PublishedContentType(Guid.NewGuid(), 666, "alias", PublishedItemType.Content, Enumerable.Empty<string>(),
                new[] {sbnFilePropertyType}, ContentVariation.Nothing);

            return new SolidPublishedContent(contentType)
            {
                Id = 1234,
                Key = Guid.NewGuid(),
                Properties = new[]
                {
                    new SolidPublishedProperty
                    {
                        Alias = "sbnFile",
                        SolidSourceValue = propertyValue,
                        SolidHasValue = true,
                        PropertyType = sbnFilePropertyType
                    }
                }
            };
        }

        private static PublishedPropertyType CreatePropertyType(string propertyEditorAlias, object dataTypeConfiguration, ContentVariation variation)
        {
            var uploadDataType = new PublishedDataType(1234, propertyEditorAlias, new Lazy<object>(() => dataTypeConfiguration));

            var propertyValueConverters = new PropertyValueConverterCollection(new IPropertyValueConverter[]
            {
                new UploadPropertyConverter(),
                new ImageCropperValueConverter(Mock.Of<ILogger<ImageCropperValueConverter>>()),
            });

            var publishedModelFactory = Mock.Of<IPublishedModelFactory>();
            var publishedContentTypeFactory = new Mock<IPublishedContentTypeFactory>();
            publishedContentTypeFactory.Setup(x => x.GetDataType(It.IsAny<int>()))
                .Returns(uploadDataType);

            return new PublishedPropertyType("sbnFile", 42, true, variation, propertyValueConverters, publishedModelFactory, publishedContentTypeFactory.Object);
        }
    }
}
