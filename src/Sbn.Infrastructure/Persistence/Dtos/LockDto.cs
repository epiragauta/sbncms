using NPoco;
using Sbn.Cms.Infrastructure.Persistence.DatabaseAnnotations;

namespace Sbn.Cms.Infrastructure.Persistence.Dtos
{
    [TableName(Cms.Core.Constants.DatabaseSchema.Tables.Lock)]
    [PrimaryKey("id", AutoIncrement = false)]
    [ExplicitColumns]
    internal class LockDto
    {
        [Column("id")]
        [PrimaryKeyColumn(Name = "PK_sbnLock", AutoIncrement = false)]
        public int Id { get; set; }

        [Column("value")]
        [NullSetting(NullSetting = NullSettings.NotNull)]
        public int Value { get; set; } = 1;

        [Column("name")]
        [NullSetting(NullSetting = NullSettings.NotNull)]
        [Length(64)]
        public string Name { get; set; }
    }
}
