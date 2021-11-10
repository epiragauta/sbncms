using NPoco;
using Sbn.Cms.Infrastructure.Persistence.DatabaseAnnotations;

namespace Sbn.Cms.Infrastructure.Persistence.Dtos
{
    [TableName(Cms.Core.Constants.DatabaseSchema.Tables.ElementTypeTree)]
    [ExplicitColumns]
    internal class ContentType2ContentTypeDto
    {
        [Column("parentContentTypeId")]
        [PrimaryKeyColumn(AutoIncrement = false, Clustered = true, Name = "PK_cmsContentType2ContentType", OnColumns = "parentContentTypeId, childContentTypeId")]
        [ForeignKey(typeof(NodeDto), Name = "FK_cmsContentType2ContentType_sbnNode_parent")]
        public int ParentId { get; set; }

        [Column("childContentTypeId")]
        [ForeignKey(typeof(NodeDto), Name = "FK_cmsContentType2ContentType_sbnNode_child")]
        public int ChildId { get; set; }
    }
}
