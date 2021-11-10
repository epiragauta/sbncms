using System.Collections.Generic;
using Sbn.Cms.Core.Features;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Editors
{
    public class BackOfficePreviewModel
    {
        private readonly SbnFeatures _features;

        public BackOfficePreviewModel(SbnFeatures features, IEnumerable<ILanguage> languages)
        {
            _features = features;
            Languages = languages;
        }

        public IEnumerable<ILanguage> Languages { get; }
        public bool DisableDevicePreview => _features.Disabled.DisableDevicePreview;
        public string PreviewExtendedHeaderView => _features.Enabled.PreviewExtendedView;
    }
}
