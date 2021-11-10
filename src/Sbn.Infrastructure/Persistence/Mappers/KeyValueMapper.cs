using System;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Infrastructure.Persistence.Dtos;

namespace Sbn.Cms.Infrastructure.Persistence.Mappers
{
    [MapperFor(typeof(KeyValue))]
    [MapperFor(typeof(IKeyValue))]
    public sealed class KeyValueMapper : BaseMapper
    {
        public KeyValueMapper(Lazy<ISqlContext> sqlContext, MapperConfigurationStore maps)
            : base(sqlContext, maps)
        { }

        protected override void DefineMaps()
        {
            DefineMap<KeyValue, KeyValueDto>(nameof(KeyValue.Identifier), nameof(KeyValueDto.Key));
            DefineMap<KeyValue, KeyValueDto>(nameof(KeyValue.Value), nameof(KeyValueDto.Value));
            DefineMap<KeyValue, KeyValueDto>(nameof(KeyValue.UpdateDate), nameof(KeyValueDto.UpdateDate));
        }
    }
}
