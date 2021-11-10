// Copyright (c) Sbn.
// See LICENSE for more details.

using System;

namespace Sbn.Cms.Tests.Common.Builders.Interfaces
{
    public interface IWithLastLoginDateBuilder
    {
        DateTime? LastLoginDate { get; set; }
    }
}
