using Microsoft.Extensions.Options;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.Models.ContentEditing;

namespace Sbn.Cms.Web.BackOffice.ModelsBuilder
{
    /// <summary>
    /// Used to validate the aliases for the content type when MB is enabled to ensure that
    /// no illegal aliases are used
    /// </summary>
    // ReSharper disable once UnusedMember.Global - This is typed scanned
    public class MediaTypeModelValidator : ContentTypeModelValidatorBase<MediaTypeSave, PropertyTypeBasic>
    {
        public MediaTypeModelValidator(IOptions<ModelsBuilderSettings> config) : base(config)
        {
        }
    }
}
