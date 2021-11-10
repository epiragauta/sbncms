using Sbn.Cms.Core.Composing;

namespace Sbn.Cms.Core.Editors
{
    public class EditorValidatorCollectionBuilder : LazyCollectionBuilderBase<EditorValidatorCollectionBuilder, EditorValidatorCollection, IEditorValidator>
    {
        protected override EditorValidatorCollectionBuilder This => this;
    }
}
