using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Sbn.Cms.Core.Cache;
using Sbn.Cms.Core.Logging;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Routing;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Web;
using Sbn.Cms.Infrastructure.Persistence;
using Sbn.Cms.Web.Website.Controllers;
using Sbn.Extensions;
using Sbn.TestData.Configuration;

namespace Sbn.TestData
{
    public class SegmentTestController : SurfaceController
    {
        private IOptions<TestDataSettings> _testDataSettings;

        public SegmentTestController(
            ISbnContextAccessor sbnContextAccessor,
            ISbnDatabaseFactory databaseFactory,
            ServiceContext services,
            AppCaches appCaches,
            IProfilingLogger profilingLogger,
            IPublishedUrlProvider publishedUrlProvider,
            IOptions<TestDataSettings> testDataSettings)
            : base(sbnContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            _testDataSettings = testDataSettings;
        }

        public IActionResult EnableDocTypeSegments(string alias, string propertyTypeAlias)
        {
            if(_testDataSettings.Value.Enabled != true)
            {
                return HttpNotFound();
            }

            IContentType ct = Services.ContentTypeService.Get(alias);
            if (ct == null)
            {
                return Content($"No document type found by alias {alias}");
            }

            IPropertyType propType = ct.PropertyTypes.FirstOrDefault(x => x.Alias == propertyTypeAlias);
            if (propType == null)
            {
                return Content($"The document type {alias} does not have a property type {propertyTypeAlias ?? "null"}");
            }

            if (ct.Variations.VariesBySegment())
            {
                return Content($"The document type {alias} already allows segments, nothing has been changed");
            }

            ct.SetVariesBy(ContentVariation.Segment);
            propType.SetVariesBy(ContentVariation.Segment);

            Services.ContentTypeService.Save(ct);
            return Content($"The document type {alias} and property type {propertyTypeAlias} now allows segments");
        }

        private IActionResult HttpNotFound() => throw new NotImplementedException();

        public IActionResult DisableDocTypeSegments(string alias)
        {
            if (_testDataSettings.Value.Enabled != true)
            {
                return HttpNotFound();
            }

            IContentType ct = Services.ContentTypeService.Get(alias);
            if (ct == null)
            {
                return Content($"No document type found by alias {alias}");
            }

            if (!ct.VariesBySegment())
            {
                return Content($"The document type {alias} does not allow segments, nothing has been changed");
            }

            ct.SetVariesBy(ContentVariation.Segment, false);

            Services.ContentTypeService.Save(ct);
            return Content($"The document type {alias} no longer allows segments");
        }

        public ActionResult AddSegmentData(int contentId, string propertyAlias, string value, string segment, string culture = null)
        {
            IContent content = Services.ContentService.GetById(contentId);
            if (content == null)
            {
                return Content($"No content found by id {contentId}");
            }

            if (propertyAlias.IsNullOrWhiteSpace() || !content.HasProperty(propertyAlias))
            {
                return Content($"The content by id {contentId} does not contain a property with alias {propertyAlias ?? "null"}");
            }

            if (content.ContentType.VariesByCulture() && culture.IsNullOrWhiteSpace())
            {
                return Content($"The content by id {contentId} varies by culture but no culture was specified");
            }

            if (value.IsNullOrWhiteSpace())
            {
                return Content("'value' cannot be null");
            }

            if (segment.IsNullOrWhiteSpace())
            {
                return Content("'segment' cannot be null");
            }

            content.SetValue(propertyAlias, value, culture, segment);
            Services.ContentService.Save(content);

            return Content($"Segment value has been set on content {contentId} for property {propertyAlias}");
        }
    }
}
