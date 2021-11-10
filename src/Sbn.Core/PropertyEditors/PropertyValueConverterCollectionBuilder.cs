using Sbn.Cms.Core.Composing;

namespace Sbn.Cms.Core.PropertyEditors
{
    public class PropertyValueConverterCollectionBuilder : OrderedCollectionBuilderBase<PropertyValueConverterCollectionBuilder, PropertyValueConverterCollection, IPropertyValueConverter>
    {
        protected override PropertyValueConverterCollectionBuilder This => this;
    }
}
