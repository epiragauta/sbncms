using System;
using System.Collections.Concurrent;

namespace Sbn.Cms.Infrastructure.Persistence.Mappers
{
    public class MapperConfigurationStore : ConcurrentDictionary<Type, ConcurrentDictionary<string, string>>
    { }
}
