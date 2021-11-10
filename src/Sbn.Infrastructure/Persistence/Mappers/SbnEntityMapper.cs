using System;
using Sbn.Cms.Core.Models.Entities;
using Sbn.Cms.Infrastructure.Persistence.Dtos;

namespace Sbn.Cms.Infrastructure.Persistence.Mappers
{
    [MapperFor(typeof (ISbnEntity))]
    public sealed class SbnEntityMapper : BaseMapper
    {
        public SbnEntityMapper(Lazy<ISqlContext> sqlContext, MapperConfigurationStore maps)
            : base(sqlContext, maps)
        { }

        protected override void DefineMaps()
        {
            DefineMap<ISbnEntity, NodeDto>(nameof(ISbnEntity.Id), nameof(NodeDto.NodeId));
            DefineMap<ISbnEntity, NodeDto>(nameof(ISbnEntity.CreateDate), nameof(NodeDto.CreateDate));
            DefineMap<ISbnEntity, NodeDto>(nameof(ISbnEntity.Level), nameof(NodeDto.Level));
            DefineMap<ISbnEntity, NodeDto>(nameof(ISbnEntity.ParentId), nameof(NodeDto.ParentId));
            DefineMap<ISbnEntity, NodeDto>(nameof(ISbnEntity.Path), nameof(NodeDto.Path));
            DefineMap<ISbnEntity, NodeDto>(nameof(ISbnEntity.SortOrder), nameof(NodeDto.SortOrder));
            DefineMap<ISbnEntity, NodeDto>(nameof(ISbnEntity.Name), nameof(NodeDto.Text));
            DefineMap<ISbnEntity, NodeDto>(nameof(ISbnEntity.Trashed), nameof(NodeDto.Trashed));
            DefineMap<ISbnEntity, NodeDto>(nameof(ISbnEntity.Key), nameof(NodeDto.UniqueId));
            DefineMap<ISbnEntity, NodeDto>(nameof(ISbnEntity.CreatorId), nameof(NodeDto.UserId));
        }
    }
}
