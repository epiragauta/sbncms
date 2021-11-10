// Copyright (c) Sbn.
// See LICENSE for more details.

using Sbn.Cms.Core.Models.PublishedContent;

namespace Sbn.Cms.Tests.Common
{
    /// <summary>
    /// Provides an implementation of <see cref="IVariationContextAccessor"/> for tests.
    /// </summary>
    public class TestVariationContextAccessor : IVariationContextAccessor
    {
        /// <inheritdoc />
        public VariationContext VariationContext { get; set; }
    }
}
