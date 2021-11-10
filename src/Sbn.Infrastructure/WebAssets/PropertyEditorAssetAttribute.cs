using System;
using Sbn.Cms.Core.WebAssets;

namespace Sbn.Cms.Infrastructure.WebAssets
{
    /// <summary>
    /// Indicates that the property editor requires this asset be loaded when the back office is loaded
    /// </summary>
    /// <remarks>
    /// This wraps a CDF asset
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    [Obsolete("Use the BackOfficeAssets collection on ISbnBuilder instead. Will be removed in the next major version")]
    public class PropertyEditorAssetAttribute : Attribute
    {
        public AssetType AssetType { get; }
        public string FilePath { get; }

        public PropertyEditorAssetAttribute(AssetType assetType, string filePath)
        {
            AssetType = assetType;
            FilePath = filePath;
        }
    }
}
