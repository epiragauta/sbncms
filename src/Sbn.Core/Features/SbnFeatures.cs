using System;

namespace Sbn.Cms.Core.Features
{
    /// <summary>
    /// Represents the Sbn features.
    /// </summary>
    public class SbnFeatures
    {
        /// <summary>
        ///  Initializes a new instance of the <see cref="SbnFeatures"/> class.
        /// </summary>
        public SbnFeatures()
        {
            Disabled = new DisabledFeatures();
            Enabled = new EnabledFeatures();
        }

        /// <summary>
        /// Gets the disabled features.
        /// </summary>
        public DisabledFeatures Disabled { get; }

        /// <summary>
        /// Gets the enabled features.
        /// </summary>
        public EnabledFeatures Enabled { get; }

        /// <summary>
        /// Determines whether a controller is enabled.
        /// </summary>
        public bool IsControllerEnabled(Type feature)
        {
            if (typeof(ISbnFeature).IsAssignableFrom(feature))
                return Disabled.Controllers.Contains(feature) == false;

            throw new NotSupportedException("Not a supported feature type.");
        }
    }
}
