using Sbn.Cms.Core.Hosting;
using Sbn.Cms.Core.Mapping;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Models.ContentEditing;
using Sbn.Cms.Core.Serialization;
using Sbn.Cms.Core.Services;

namespace Sbn.Cms.Web.BackOffice.ModelBinders
{
    internal class BlueprintItemBinder : ContentItemBinder
    {
        private readonly IContentService _contentService;

        public BlueprintItemBinder(IJsonSerializer jsonSerializer, ISbnMapper sbnMapper, IContentService contentService, IContentTypeService contentTypeService, IHostingEnvironment hostingEnvironment) : base(jsonSerializer, sbnMapper, contentService, contentTypeService, hostingEnvironment)
        {
            _contentService = contentService;
        }

        protected override IContent GetExisting(ContentItemSave model)
        {
            return _contentService.GetBlueprintById(model.Id);
        }
    }
}
