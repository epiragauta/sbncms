using NPoco;
using Sbn.Cms.Infrastructure.Persistence.DatabaseAnnotations;

namespace Sbn.Cms.Infrastructure.Persistence.Dtos
{
    [TableName(Cms.Core.Constants.DatabaseSchema.Tables.Template)]
    [PrimaryKey("pk")]
    [ExplicitColumns]
    internal class TemplateDto
    {
        [Column("pk")]
        [PrimaryKeyColumn]
        public int PrimaryKey { get; set; }

        [Column("nodeId")]
        [Index(IndexTypes.UniqueNonClustered)]
        [ForeignKey(typeof(NodeDto), Name = "FK_cmsTemplate_sbnNode")]
        public int NodeId { get; set; }

        [Column("alias")]
        [Length(100)]
        [NullSetting(NullSetting = NullSettings.Null)]
        public string Alias { get; set; }

        [ResultColumn]
        [Reference(ReferenceType.OneToOne, ColumnName = "NodeId")]
        public NodeDto NodeDto { get; set; }
    }
}
