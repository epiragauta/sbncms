using Sbn.Cms.Core.Composing;

namespace Sbn.Cms.Core.PropertyEditors
{
    public class ManifestValueValidatorCollectionBuilder : SetCollectionBuilderBase<ManifestValueValidatorCollectionBuilder, ManifestValueValidatorCollection, IManifestValueValidator>
    {
        protected override ManifestValueValidatorCollectionBuilder This => this;
    }
}
