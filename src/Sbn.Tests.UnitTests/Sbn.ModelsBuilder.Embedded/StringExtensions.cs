// Copyright (c) Sbn.
// See LICENSE for more details.

namespace Sbn.Cms.Tests.UnitTests.Sbn.ModelsBuilder.Embedded
{
    public static class StringExtensions
    {
        public static string ClearLf(this string s) => s.Replace("\r", string.Empty);
    }
}
