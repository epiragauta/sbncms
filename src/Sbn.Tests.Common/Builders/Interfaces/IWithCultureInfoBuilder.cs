// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Globalization;

namespace Sbn.Cms.Tests.Common.Builders.Interfaces
{
    public interface IWithCultureInfoBuilder
    {
        CultureInfo CultureInfo { get; set; }
    }
}
