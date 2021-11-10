using System.Collections.Generic;

namespace Sbn.Cms.Core.PropertyEditors
{
    /// <summary>
    /// Marker interface for any editor configuration that supports defining file extensions
    /// </summary>
    public interface IFileExtensionsConfig
    {
        List<FileExtensionConfigItem> FileExtensions { get; set; }
    }
}
