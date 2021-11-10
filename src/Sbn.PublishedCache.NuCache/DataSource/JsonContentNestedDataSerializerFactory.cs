using System;

namespace Sbn.Cms.Infrastructure.PublishedCache.DataSource
{
    internal class JsonContentNestedDataSerializerFactory : IContentCacheDataSerializerFactory
    {
        private readonly Lazy<JsonContentNestedDataSerializer> _serializer = new Lazy<JsonContentNestedDataSerializer>();
        public IContentCacheDataSerializer Create(ContentCacheDataSerializerEntityType types) => _serializer.Value;
    }
}
