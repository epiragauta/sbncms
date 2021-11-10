// Copyright (c) Sbn.
// See LICENSE for more details.

using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Tests.Common.Builders.Interfaces
{
    public interface IWithParentContentTypeBuilder
    {
        IContentTypeComposition Parent { get; set; }
    }
}
