using Sbn.Cms.Core.Composing;

namespace Sbn.Cms.Core.PropertyEditors
{
    public class DataValueReferenceFactoryCollectionBuilder : OrderedCollectionBuilderBase<DataValueReferenceFactoryCollectionBuilder, DataValueReferenceFactoryCollection, IDataValueReferenceFactory>
    {
        protected override DataValueReferenceFactoryCollectionBuilder This => this;
    }
}
