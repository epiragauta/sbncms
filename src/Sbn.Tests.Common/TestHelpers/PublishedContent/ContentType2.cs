// Copyright (c) Sbn.
// See LICENSE for more details.

using Moq;
using Sbn.Cms.Core.Models.PublishedContent;
using Sbn.Extensions;

namespace Sbn.Cms.Tests.Common.TestHelpers.PublishedContent
{
    [PublishedModel("ContentType2")]
    public class ContentType2 : PublishedContentModel
    {
        public ContentType2(IPublishedContent content, IPublishedValueFallback fallback)
            : base(content, fallback)
        {
        }

        public int Prop1 => this.Value<int>(Mock.Of<IPublishedValueFallback>(), "prop1");
    }
}
