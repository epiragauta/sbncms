// Copyright (c) Sbn.
// See LICENSE for more details.

using System.IO;
using NUnit.Framework;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Routing;
using Sbn.Cms.Tests.Common.Builders;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Tests.Common.Builders
{
    [TestFixture]
    public class StylesheetBuilderTests
    {
        [Test]
        public void Is_Built_Correctly()
        {
            // Arrange
            var testPath = WebPath.PathSeparator + WebPath.Combine("css", "styles.css");
            const string testContent = @"body { color:#000; } .bold {font-weight:bold;}";

            var builder = new StylesheetBuilder();

            // Act
            Stylesheet stylesheet = builder
                .WithPath(testPath)
                .WithContent(testContent)
                .Build();

            // Assert
            Assert.AreEqual(Path.DirectorySeparatorChar + Path.Combine("css", "styles.css"), stylesheet.Path);
            Assert.AreEqual(testContent, stylesheet.Content);
        }
    }
}
