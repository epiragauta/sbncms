using Sbn.Cms.Core.Composing;

namespace Sbn.Cms.Core.WebAssets
{
    public class CustomBackOfficeAssetsCollectionBuilder : OrderedCollectionBuilderBase<CustomBackOfficeAssetsCollectionBuilder, CustomBackOfficeAssetsCollection, IAssetFile>
    {
        protected override CustomBackOfficeAssetsCollectionBuilder This => this;
    }
}
