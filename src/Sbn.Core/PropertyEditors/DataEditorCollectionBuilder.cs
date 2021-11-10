using Sbn.Cms.Core.Composing;

namespace Sbn.Cms.Core.PropertyEditors
{
    public class DataEditorCollectionBuilder : LazyCollectionBuilderBase<DataEditorCollectionBuilder, DataEditorCollection, IDataEditor>
    {
        protected override DataEditorCollectionBuilder This => this;
    }
}
