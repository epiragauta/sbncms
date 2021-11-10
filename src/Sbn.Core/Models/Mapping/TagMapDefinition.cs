using Sbn.Cms.Core.Mapping;

namespace Sbn.Cms.Core.Models.Mapping
{
    public class TagMapDefinition : IMapDefinition
    {
        public void DefineMaps(ISbnMapper mapper)
        {
            mapper.Define<ITag, TagModel>((source, context) => new TagModel(), Map);
        }

        // Sbn.Code.MapAll
        private static void Map(ITag source, TagModel target, MapperContext context)
        {
            target.Id = source.Id;
            target.Text = source.Text;
            target.Group = source.Group;
            target.NodeCount = source.NodeCount;
        }
    }
}
