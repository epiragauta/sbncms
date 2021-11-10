using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Sbn.Cms.Core.Hosting;
using Sbn.Cms.Core.Mapping;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Models.ContentEditing;
using Sbn.Cms.Core.Serialization;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Web.BackOffice.Controllers;

namespace Sbn.Cms.Web.BackOffice.ModelBinders
{
    /// <summary>
    /// The model binder for <see cref="T:Sbn.Web.Models.ContentEditing.MediaItemSave" />
    /// </summary>
    internal class MediaItemBinder : IModelBinder
    {
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMediaService _mediaService;
        private readonly ISbnMapper _sbnMapper;
        private readonly IMediaTypeService _mediaTypeService;
        private readonly ContentModelBinderHelper _modelBinderHelper;


        public MediaItemBinder(IJsonSerializer jsonSerializer, IHostingEnvironment hostingEnvironment, IMediaService mediaService, ISbnMapper sbnMapper, IMediaTypeService mediaTypeService)
        {
            _jsonSerializer = jsonSerializer ?? throw new ArgumentNullException(nameof(jsonSerializer));
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
            _mediaService = mediaService ?? throw new ArgumentNullException(nameof(mediaService));
            _sbnMapper = sbnMapper ?? throw new ArgumentNullException(nameof(sbnMapper));
            _mediaTypeService = mediaTypeService ?? throw new ArgumentNullException(nameof(mediaTypeService));

            _modelBinderHelper = new ContentModelBinderHelper();
        }

        /// <summary>
        /// Creates the model from the request and binds it to the context
        /// </summary>
        /// <param name="bindingContext"></param>
        /// <returns></returns>
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {

            var model = await _modelBinderHelper.BindModelFromMultipartRequestAsync<MediaItemSave>(_jsonSerializer, _hostingEnvironment, bindingContext);
            if (model == null)
            {
                return;
            }

            model.PersistedContent = ContentControllerBase.IsCreatingAction(model.Action) ? CreateNew(model) : GetExisting(model);

            //create the dto from the persisted model
            if (model.PersistedContent != null)
            {
                model.PropertyCollectionDto = _sbnMapper.Map<IMedia, ContentPropertyCollectionDto>(model.PersistedContent);
                //now map all of the saved values to the dto
                _modelBinderHelper.MapPropertyValuesFromSaved(model, model.PropertyCollectionDto);
            }

            model.Name = model.Name.Trim();

            bindingContext.Result = ModelBindingResult.Success(model);
        }

        private IMedia GetExisting(MediaItemSave model)
        {
            return _mediaService.GetById(Convert.ToInt32(model.Id));
        }

        private IMedia CreateNew(MediaItemSave model)
        {
            var mediaType = _mediaTypeService.Get(model.ContentTypeAlias);
            if (mediaType == null)
            {
                throw new InvalidOperationException("No media type found with alias " + model.ContentTypeAlias);
            }
            return new Cms.Core.Models.Media(model.Name, model.ParentId, mediaType);
        }

    }
}
