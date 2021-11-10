using Sbn.Cms.Core.Mapping;
using Sbn.Cms.Core.Models.ContentEditing;

namespace Sbn.Cms.Core.Models.Mapping
{
    public class AuditMapDefinition : IMapDefinition
    {
        public void DefineMaps(ISbnMapper mapper)
        {
            mapper.Define<IAuditItem, AuditLog>((source, context) => new AuditLog(), Map);
        }

        // Sbn.Code.MapAll -UserAvatars -UserName
        private void Map(IAuditItem source, AuditLog target, MapperContext context)
        {
            target.UserId = source.UserId;
            target.NodeId = source.Id;
            target.Timestamp = source.CreateDate;
            target.LogType = source.AuditType.ToString();
            target.EntityType = source.EntityType;
            target.Comment = source.Comment;
            target.Parameters = source.Parameters;
        }
    }
}
