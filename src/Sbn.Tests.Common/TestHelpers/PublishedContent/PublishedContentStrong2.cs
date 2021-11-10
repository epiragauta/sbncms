// Copyright (c) Sbn.
// See LICENSE for more details.

using Moq;
using Sbn.Cms.Core.Models.PublishedContent;
using Sbn.Extensions;

namespace Sbn.Cms.Tests.Common.TestHelpers.PublishedContent
{
    public class PublishedContentStrong2 : PublishedContentModel
    {
        public PublishedContentStrong2(IPublishedContent content, IPublishedValueFallback fallback)
            : base(content, fallback)
        {
        }

        public int StrongValue => this.Value<int>(Mock.Of<IPublishedValueFallback>(), "strongValue");
    }
}
