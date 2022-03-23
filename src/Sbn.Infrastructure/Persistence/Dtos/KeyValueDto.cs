using System;
using NPoco;
using Sbn.Cms.Infrastructure.Persistence.DatabaseAnnotations;
using Sbn.Cms.Infrastructure.Persistence.DatabaseModelDefinitions;

namespace Sbn.Cms.Infrastructure.Persistence.Dtos
{
    [TableName(Cms.Core.Constants.DatabaseSchema.Tables.KeyValue)]
    [PrimaryKey("key", AutoIncrement = false)]
    [ExplicitColumns]
    public class KeyValueDto
    {
        [Column("key")]
        [Length(256)]
        [PrimaryKeyColumn(AutoIncrement = false, Clustered = true)]
        public string Key { get; set; }

        [Column("value")]
        [NullSetting(NullSetting = NullSettings.Null)]
        public string Value { get; set; }

        [Column("updated")]
        [Constraint(Default = SystemMethods.CurrentDateTime)]
        public DateTime UpdateDate { get; set; }
    }
}
