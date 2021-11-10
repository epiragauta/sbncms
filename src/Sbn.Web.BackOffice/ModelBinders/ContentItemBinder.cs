using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Sbn.Cms.Core.Hosting;
using Sbn.Cms.Core.Mapping;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Models.ContentEditing;
using Sbn.Cms.Core.Serialization;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Web.BackOffice.Controllers;
using Sbn.Extensions;

namespace Sbn.Cms.Web.BackOffice.ModelBinders
{
    /// <summary>
    /// The model binder for <see cref="T:Sbn.Web.Models.ContentEditing.ContentItemSave" />
    /// </summary>
    internal class ContentItemBinder : IModelBinder
    {
        private readonly IJsonSerializer _jsonSerializer;
        private readonly ISbnMapper _sbnMapper;
        private readonly IContentService _contentService;
        private readonly IContentTypeService _contentTypeService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private ContentModelBinderHelper _modelBinderHelper;

        public ContentItemBinder(
            IJsonSerializer jsonSerializer,
            ISbnMapper sbnMapper,
            IContentService contentService,
            IContentTypeService contentTypeService,
            IHostingEnvironment hostingEnvironment)
        {
            _jsonSerializer = jsonSerializer ?? throw new ArgumentNullException(nameof(jsonSerializer));
            _sbnMapper = sbnMapper ?? throw new ArgumentNullException(nameof(sbnMapper));
            _contentService = contentService ?? throw new ArgumentNullException(nameof(contentService));
            _contentTypeService = contentTypeService ?? throw new ArgumentNullException(nameof(contentTypeService));
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
            _modelBinderHelper = new ContentModelBinderHelper();
        }

        protected virtual IContent GetExisting(ContentItemSave model)
        {
            return _contentService.GetById(model.Id);
        }

        private IContent CreateNew(ContentItemSave model)
        {
            var contentType = _contentTypeService.Get(model.ContentTypeAlias);
            if (contentType == null)
            {
                throw new InvalidOperationException("No content type found with alias " + model.ContentTypeAlias);
            }
            return new Content(
                contentType.VariesByCulture() ? null : model.Variants.First().Name,
                model.ParentId,
                contentType);
        }


        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var model = await _modelBinderHelper.BindModelFromMultipartRequestAsync<ContentItemSave>(_jsonSerializer, _hostingEnvironment, bindingContext);

            if (model is null)
            {
                return;
            }

            var persistedContent = ContentControllerBase.IsCreatingAction(model.Action) ? CreateNew(model) : GetExisting(model);
            BindModel(model, persistedContent, _modelBinderHelper, _sbnMapper);

            bindingContext.Result = ModelBindingResult.Success(model);
        }

        internal static void BindModel(ContentItemSave model, IContent persistedContent, ContentModelBinderHelper modelBinderHelper, ISbnMapper sbnMapper)
        {
            model.PersistedContent =persistedContent;

            //create the dto from the persisted model
            if (model.PersistedContent != null)
            {
                foreach (var variant in model.Variants)
                {
                    //map the property dto collection with the culture of the current variant
                    variant.PropertyCollectionDto = sbnMapper.Map<ContentPropertyCollectionDto>(
                        model.PersistedContent,
                        context =>
                        {
                            // either of these may be null and that is ok, if it's invariant they will be null which is what is expected
                            context.SetCulture(variant.Culture);
                            context.SetSegment(variant.Segment);
                        });

                    //now map all of the saved values to the dto
                    modelBinderHelper.MapPropertyValuesFromSaved(variant, variant.PropertyCollectionDto);
                }
            }
        }
    }
}
