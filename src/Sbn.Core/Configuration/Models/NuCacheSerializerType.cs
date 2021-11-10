// Copyright (c) Sbn.
// See LICENSE for more details.

namespace Sbn.Cms.Core.Configuration.Models
{
    /// <summary>
    /// The serializer type that nucache uses to persist documents in the database.
    /// </summary>
    public enum NuCacheSerializerType
    {
        MessagePack = 1, // Default
        JSON = 2
    }
}
