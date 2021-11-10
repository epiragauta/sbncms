using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.PropertyEditors
{
    /// <summary>
    /// Default implementation for <see cref="IPropertyCacheCompressionOptions"/> which does not compress any property data
    /// </summary>
    public sealed class NoopPropertyCacheCompressionOptions : IPropertyCacheCompressionOptions
    {
        public bool IsCompressed(IReadOnlyContentBase content, IPropertyType propertyType, IDataEditor dataEditor, bool published) => false;
    }
}
