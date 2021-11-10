using System;
using NPoco;
using Sbn.Cms.Infrastructure.Persistence.DatabaseAnnotations;
using Sbn.Cms.Infrastructure.Persistence.DatabaseModelDefinitions;

namespace Sbn.Cms.Infrastructure.Persistence.Dtos
{
    [TableName(Cms.Core.Constants.DatabaseSchema.Tables.Relation)]
    [PrimaryKey("id")]
    [ExplicitColumns]
    internal class RelationDto
    {
        [Column("id")]
        [PrimaryKeyColumn]
        public int Id { get; set; }

        [Column("parentId")]
        [ForeignKey(typeof(NodeDto), Name = "FK_sbnRelation_sbnNode")]
        [Index(IndexTypes.UniqueNonClustered, Name = "IX_sbnRelation_parentChildType", ForColumns = "parentId,childId,relType")]
        public int ParentId { get; set; }

        [Column("childId")]
        [ForeignKey(typeof(NodeDto), Name = "FK_sbnRelation_sbnNode1")]
        public int ChildId { get; set; }

        [Column("relType")]
        [ForeignKey(typeof(RelationTypeDto))]
        public int RelationType { get; set; }

        [Column("datetime")]
        [Constraint(Default = SystemMethods.CurrentDateTime)]
        public DateTime Datetime { get; set; }

        [Column("comment")]
        [Length(1000)]
        public string Comment { get; set; }

        [ResultColumn]
        [Column("parentObjectType")]
        public Guid ParentObjectType { get; set; }

        [ResultColumn]
        [Column("childObjectType")]
        public Guid ChildObjectType { get; set; }
    }
}
