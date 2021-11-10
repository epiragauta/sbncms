// Copyright (c) Sbn.
// See LICENSE for more details.

using Moq;
using Sbn.Cms.Core.Models.PublishedContent;
using Sbn.Extensions;

namespace Sbn.Cms.Tests.Common.TestHelpers.PublishedContent
{
    public class PublishedContentStrong1Sub : PublishedContentStrong1
    {
        public PublishedContentStrong1Sub(IPublishedContent content, IPublishedValueFallback fallback)
            : base(content, fallback)
        {
        }

        public int AnotherValue => this.Value<int>(Mock.Of<IPublishedValueFallback>(), "anotherValue");
    }
}
