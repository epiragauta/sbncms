using System;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Infrastructure.Persistence.Dtos;

namespace Sbn.Cms.Infrastructure.Persistence.Mappers
{
    [MapperFor(typeof(Property))]
    public sealed class PropertyMapper : BaseMapper
    {
        public PropertyMapper(Lazy<ISqlContext> sqlContext, MapperConfigurationStore maps)
            : base(sqlContext, maps)
        { }

        protected override void DefineMaps()
        {
            DefineMap<Property, PropertyDataDto>(nameof(Property.Id), nameof(PropertyDataDto.Id));
            DefineMap<Property, PropertyDataDto>(nameof(Property.PropertyTypeId), nameof(PropertyDataDto.PropertyTypeId));
        }
    }
}
