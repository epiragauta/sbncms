using Sbn.Cms.Core.Manifest;
using Sbn.Cms.Core.Mapping;
using Sbn.Cms.Core.Models.ContentEditing;
using Sbn.Cms.Core.Sections;
using Sbn.Cms.Core.Services;
using Sbn.Extensions;

namespace Sbn.Cms.Core.Models.Mapping
{
    public class SectionMapDefinition : IMapDefinition
    {
        private readonly ILocalizedTextService _textService;
        public SectionMapDefinition(ILocalizedTextService textService)
        {
            _textService = textService;
        }

        public void DefineMaps(ISbnMapper mapper)
        {
            mapper.Define<ISection, Section>((source, context) => new Section(), Map);

            // this is for AutoMapper ReverseMap - but really?
            mapper.Define<Section, ContentSection>();
            mapper.Define<Section, ContentSection>();
            mapper.Define<Section, ManifestSection>(Map);
            mapper.Define<Section, MediaSection>();
            mapper.Define<Section, MembersSection>();
            mapper.Define<Section, PackagesSection>();
            mapper.Define<Section, SettingsSection>();
            mapper.Define<Section, TranslationSection>();
            mapper.Define<Section, UsersSection>();
        }

        // Sbn.Code.MapAll -RoutePath
        private void Map(ISection source, Section target, MapperContext context)
        {
            target.Alias = source.Alias;
            target.Name = _textService.Localize("sections", source.Alias);
        }

        // Sbn.Code.MapAll
        private static void Map(Section source, ManifestSection target, MapperContext context)
        {
            target.Alias = source.Alias;
            target.Name = source.Name;
        }
    }
}
