using Microsoft.Extensions.Logging;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Models.ContentEditing;
using Sbn.Cms.Core.Services;

namespace Sbn.Cms.Web.BackOffice.Filters
{
    /// <summary>
    /// Validator for <see cref="ContentItemSave"/>
    /// </summary>
    internal class ContentSaveModelValidator : ContentModelValidator<IContent, ContentItemSave, ContentVariantSave>
    {
        public ContentSaveModelValidator(
            ILogger<ContentSaveModelValidator> logger,
            IPropertyValidationService propertyValidationService)
            : base(logger, propertyValidationService)
        {
        }

    }
}
