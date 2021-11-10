// Copyright (c) Sbn.
// See LICENSE for more details.

using Sbn.Cms.Core.Models.PublishedContent;

namespace Sbn.Cms.Tests.Common.TestHelpers.PublishedContent
{
    [PublishedModel("ContentType2Sub")]
    public class ContentType2Sub : ContentType2
    {
        public ContentType2Sub(IPublishedContent content, IPublishedValueFallback fallback)
            : base(content, fallback)
        {
        }
    }
}
