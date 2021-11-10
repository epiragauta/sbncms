using System;
using System.Collections.Generic;
using NPoco;
using Sbn.Cms.Infrastructure.Persistence.DatabaseAnnotations;
using Sbn.Cms.Infrastructure.Persistence.DatabaseModelDefinitions;

namespace Sbn.Cms.Infrastructure.Persistence.Dtos
{
    [TableName(Cms.Core.Constants.DatabaseSchema.Tables.Access)]
    [PrimaryKey("id", AutoIncrement = false)]
    [ExplicitColumns]
    internal class AccessDto
    {
        [Column("id")]
        [PrimaryKeyColumn(Name = "PK_sbnAccess", AutoIncrement = false)]
        public Guid Id { get; set; }

        [Column("nodeId")]
        [ForeignKey(typeof(NodeDto), Name = "FK_sbnAccess_sbnNode_id")]
        [Index(IndexTypes.UniqueNonClustered, Name = "IX_sbnAccess_nodeId")]
        public int NodeId { get; set; }

        [Column("loginNodeId")]
        [ForeignKey(typeof(NodeDto), Name = "FK_sbnAccess_sbnNode_id1")]
        public int LoginNodeId { get; set; }

        [Column("noAccessNodeId")]
        [ForeignKey(typeof(NodeDto), Name = "FK_sbnAccess_sbnNode_id2")]
        public int NoAccessNodeId { get; set; }

        [Column("createDate")]
        [Constraint(Default = SystemMethods.CurrentDateTime)]
        public DateTime CreateDate { get; set; }

        [Column("updateDate")]
        [Constraint(Default = SystemMethods.CurrentDateTime)]
        public DateTime UpdateDate { get; set; }

        [ResultColumn]
        [Reference(ReferenceType.Many, ReferenceMemberName = "AccessId")]
        public List<AccessRuleDto> Rules { get; set; }
    }
}
