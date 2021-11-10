using Microsoft.Extensions.Logging;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Models.ContentEditing;
using Sbn.Cms.Core.Services;

namespace Sbn.Cms.Web.BackOffice.Filters
{
    /// <summary>
    /// Validator for <see cref="MediaItemSave"/>
    /// </summary>
    internal class MediaSaveModelValidator : ContentModelValidator<IMedia, MediaItemSave, IContentProperties<ContentPropertyBasic>>
    {
        public MediaSaveModelValidator(
            ILogger<MediaSaveModelValidator> logger,
            IPropertyValidationService propertyValidationService)
            : base(logger, propertyValidationService)
        {
        }
    }
}
