using Sbn.Cms.Core.Mapping;
using Sbn.Cms.Core.Models.ContentEditing;
using Sbn.Cms.Core.Services;
using Sbn.Extensions;

namespace Sbn.Cms.Core.Models.Mapping
{
    public class RelationMapDefinition : IMapDefinition
    {
        private readonly IEntityService _entityService;
        private readonly IRelationService _relationService;

        public RelationMapDefinition(IEntityService entityService, IRelationService relationService)
        {
            _entityService = entityService;
            _relationService = relationService;
        }

        public void DefineMaps(ISbnMapper mapper)
        {
            mapper.Define<IRelationType, RelationTypeDisplay>((source, context) => new RelationTypeDisplay(), Map);
            mapper.Define<IRelation, RelationDisplay>((source, context) => new RelationDisplay(), Map);
            mapper.Define<RelationTypeSave, IRelationType>(Map);
        }

        // Sbn.Code.MapAll -Icon -Trashed -AdditionalData
        // Sbn.Code.MapAll -ParentId -Notifications
        private void Map(IRelationType source, RelationTypeDisplay target, MapperContext context)
        {
            target.ChildObjectType = source.ChildObjectType;
            target.Id = source.Id;
            target.IsBidirectional = source.IsBidirectional;
            target.Key = source.Key;
            target.Name = source.Name;
            target.Alias = source.Alias;
            target.ParentObjectType = source.ParentObjectType;
            target.Udi = Udi.Create(Constants.UdiEntityType.RelationType, source.Key);
            target.Path = "-1," + source.Id;

            target.IsSystemRelationType = source.IsSystemRelationType();

            // Set the "friendly" and entity names for the parent and child object types
            if (source.ParentObjectType.HasValue)
            {
                var objType = ObjectTypes.GetSbnObjectType(source.ParentObjectType.Value);
                target.ParentObjectTypeName = objType.GetFriendlyName();
            }

            if (source.ChildObjectType.HasValue)
            {
                var objType = ObjectTypes.GetSbnObjectType(source.ChildObjectType.Value);
                target.ChildObjectTypeName = objType.GetFriendlyName();
            }
        }

        // Sbn.Code.MapAll -ParentName -ChildName
        private void Map(IRelation source, RelationDisplay target, MapperContext context)
        {
            target.ChildId = source.ChildId;
            target.Comment = source.Comment;
            target.CreateDate = source.CreateDate;
            target.ParentId = source.ParentId;

            var entities = _relationService.GetEntitiesFromRelation(source);

            target.ParentName = entities.Item1.Name;
            target.ChildName = entities.Item2.Name;
        }

        // Sbn.Code.MapAll -CreateDate -UpdateDate -DeleteDate
        private static void Map(RelationTypeSave source, IRelationType target, MapperContext context)
        {
            target.Alias = source.Alias;
            target.ChildObjectType = source.ChildObjectType;
            target.Id = source.Id.TryConvertTo<int>().Result;
            target.IsBidirectional = source.IsBidirectional;
            target.Key = source.Key;
            target.Name = source.Name;
            target.ParentObjectType = source.ParentObjectType;
        }
    }
}
