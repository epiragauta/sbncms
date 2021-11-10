// Copyright (c) Sbn.
// See LICENSE for more details.

using NUnit.Framework;
using Sbn.Cms.Core.Cache;
using Sbn.Cms.Core.Macros;
using Sbn.Cms.Web.Common.Macros;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Web.Common.Macros
{
    [TestFixture]
    public class MacroTests
    {
        [SetUp]
        public void Setup()
        {
            // We DO want cache enabled for these tests
            var cacheHelper = new AppCaches(
                new ObjectCacheAppCache(),
                NoAppCache.Instance,
                new IsolatedCaches(type => new ObjectCacheAppCache()));
        }

        [TestCase("anything", true)]
        [TestCase("", false)]
        public void Macro_Is_File_Based(string macroSource, bool expectedNonNull)
        {
            var model = new MacroModel
            {
                MacroSource = macroSource
            };
            var filename = MacroRenderer.GetMacroFileName(model);
            if (expectedNonNull)
            {
                Assert.IsNotNull(filename);
            }
            else
            {
                Assert.IsNull(filename);
            }
        }
    }
}
