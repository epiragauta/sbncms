// Copyright (c) Sbn.
// See LICENSE for more details.

using Sbn.Cms.Core.PublishedCache;

namespace Sbn.Cms.Tests.Common
{
    public class TestDefaultCultureAccessor : IDefaultCultureAccessor
    {
        private string _defaultCulture = string.Empty;

        public string DefaultCulture
        {
            get => _defaultCulture;
            set => _defaultCulture = value ?? string.Empty;
        }
    }
}
