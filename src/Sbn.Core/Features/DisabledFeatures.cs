using Sbn.Cms.Core.Collections;

namespace Sbn.Cms.Core.Features
{
    /// <summary>
    /// Represents disabled features.
    /// </summary>
    public class DisabledFeatures
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DisabledFeatures"/> class.
        /// </summary>
        public DisabledFeatures()
        {
            Controllers = new TypeList<ISbnFeature>();
        }

        /// <summary>
        /// Gets the disabled controllers.
        /// </summary>
        public TypeList<ISbnFeature> Controllers { get; }

        /// <summary>
        /// Disables the device preview feature of previewing.
        /// </summary>
        public bool DisableDevicePreview { get; set; }

        /// <summary>
        /// If true, all references to templates will be removed in the back office and routing
        /// </summary>
        public bool DisableTemplates { get; set; }

    }
}
