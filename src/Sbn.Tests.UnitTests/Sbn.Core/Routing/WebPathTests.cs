// Copyright (c) Sbn.
// See LICENSE for more details.

using System;
using NUnit.Framework;
using Sbn.Cms.Core.Routing;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Core.Routing
{
    [TestFixture]
    public class WebPathTests
    {
        [Test]
        [TestCase("/sbn", "config", "lang", ExpectedResult = "/sbn/config/lang")]
        [TestCase("/sbn", "/config", "/lang", ExpectedResult = "/sbn/config/lang")]
        [TestCase("/sbn/", "/config/", "/lang/", ExpectedResult = "/sbn/config/lang")]
        [TestCase("/sbn/", "config/", "lang/", ExpectedResult = "/sbn/config/lang")]
        [TestCase("sbn", "config", "lang", ExpectedResult = "sbn/config/lang")]
        [TestCase("sbn", ExpectedResult = "sbn")]
        [TestCase("~/sbn", "config", "lang", ExpectedResult = "~/sbn/config/lang")]
        [TestCase("~/sbn", "/config", "/lang", ExpectedResult = "~/sbn/config/lang")]
        [TestCase("~/sbn/", "/config/", "/lang/", ExpectedResult = "~/sbn/config/lang")]
        [TestCase("~/sbn/", "config/", "lang/", ExpectedResult = "~/sbn/config/lang")]
        [TestCase("~/sbn", ExpectedResult = "~/sbn")]
        [TestCase("https://hello.com/", "/world", ExpectedResult = "https://hello.com/world")]
        public string Combine(params string[] parts) => WebPath.Combine(parts);

        [Test]
        public void Combine_must_handle_empty_array() => Assert.AreEqual(string.Empty, WebPath.Combine(Array.Empty<string>()));

        [Test]
        public void Combine_must_handle_null() => Assert.Throws<ArgumentNullException>(() => WebPath.Combine(null));
    }
}
