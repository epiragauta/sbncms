// Copyright (c) Sbn.
// See LICENSE for more details.

using System;

namespace Sbn.Cms.Tests.Common.Builders.Interfaces
{
    public interface IWithIsLockedOutBuilder
    {
        bool? IsLockedOut { get; set; }

        DateTime? LastLockoutDate { get; set; }
    }
}
