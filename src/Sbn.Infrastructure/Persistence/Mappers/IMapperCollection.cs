using System;
using Sbn.Cms.Core.Composing;

namespace Sbn.Cms.Infrastructure.Persistence.Mappers
{
    public interface IMapperCollection : IBuilderCollection<BaseMapper>
    {
        bool TryGetMapper(Type type, out BaseMapper mapper);
        BaseMapper this[Type type] { get; }
    }
}
