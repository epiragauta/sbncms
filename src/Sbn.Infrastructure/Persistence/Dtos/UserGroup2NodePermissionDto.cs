using NPoco;
using Sbn.Cms.Infrastructure.Persistence.DatabaseAnnotations;

namespace Sbn.Cms.Infrastructure.Persistence.Dtos
{
    [TableName(Cms.Core.Constants.DatabaseSchema.Tables.UserGroup2NodePermission)]
    [ExplicitColumns]
    internal class UserGroup2NodePermissionDto
    {
        [Column("userGroupId")]
        [PrimaryKeyColumn(AutoIncrement = false, Name = "PK_sbnUserGroup2NodePermission", OnColumns = "userGroupId, nodeId, permission")]
        [ForeignKey(typeof(UserGroupDto))]
        public int UserGroupId { get; set; }

        [Column("nodeId")]
        [ForeignKey(typeof(NodeDto))]
        [Index(IndexTypes.NonClustered, Name = "IX_sbnUser2NodePermission_nodeId")]
        public int NodeId { get; set; }

        [Column("permission")]
        public string Permission { get; set; }
    }
}
