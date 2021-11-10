using System;
using NPoco;
using Sbn.Cms.Infrastructure.Persistence.DatabaseAnnotations;
using Sbn.Cms.Infrastructure.Persistence.DatabaseModelDefinitions;

namespace Sbn.Cms.Infrastructure.Persistence.Dtos
{
    [TableName(Cms.Core.Constants.DatabaseSchema.Tables.AccessRule)]
    [PrimaryKey("id", AutoIncrement = false)]
    [ExplicitColumns]
    internal class AccessRuleDto
    {
        [Column("id")]
        [PrimaryKeyColumn(Name = "PK_sbnAccessRule", AutoIncrement = false)]
        public Guid Id { get; set; }

        [Column("accessId")]
        [ForeignKey(typeof(AccessDto), Name = "FK_sbnAccessRule_sbnAccess_id")]
        public Guid AccessId { get; set; }

        [Column("ruleValue")]
        [Index(IndexTypes.UniqueNonClustered, ForColumns = "ruleValue,ruleType,accessId", Name = "IX_sbnAccessRule")]
        public string RuleValue { get; set; }

        [Column("ruleType")]
        public string RuleType { get; set; }

        [Column("createDate")]
        [Constraint(Default = SystemMethods.CurrentDateTime)]
        public DateTime CreateDate { get; set; }

        [Column("updateDate")]
        [Constraint(Default = SystemMethods.CurrentDateTime)]
        public DateTime UpdateDate { get; set; }
    }
}
