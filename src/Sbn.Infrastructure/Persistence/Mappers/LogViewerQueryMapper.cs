using System;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Infrastructure.Persistence.Dtos;

namespace Sbn.Cms.Infrastructure.Persistence.Mappers
{
    [MapperFor(typeof(ILogViewerQuery))]
    [MapperFor(typeof(LogViewerQuery))]
    public sealed class LogViewerQueryMapper : BaseMapper
    {
        public LogViewerQueryMapper(Lazy<ISqlContext> sqlContext, MapperConfigurationStore maps)
            : base(sqlContext, maps)
        { }

        protected override void DefineMaps()
        {
            DefineMap<ILogViewerQuery, LogViewerQueryDto>(nameof(ILogViewerQuery.Id), nameof(LogViewerQueryDto.Id));
            DefineMap<ILogViewerQuery, LogViewerQueryDto>(nameof(ILogViewerQuery.Name), nameof(LogViewerQueryDto.Name));
            DefineMap<ILogViewerQuery, LogViewerQueryDto>(nameof(ILogViewerQuery.Query), nameof(LogViewerQueryDto.Query));
        }
    }
}
