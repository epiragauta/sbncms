// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Xml;
using NUnit.Framework;
using Sbn.Cms.Tests.Common.Builders;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Tests.Common.Builders
{
    [TestFixture]
    public class XmlDocumentBuilderTests
    {
        [Test]
        public void Is_Built_Correctly()
        {
            // Arrange
            const string content =
                @"<?xml version=""1.0"" encoding=""utf-8""?><root id=""-1""></root>";

            var builder = new XmlDocumentBuilder();

            // Act
            XmlDocument xml = builder
                .WithContent(content)
                .Build();

            // Assert
            Assert.AreEqual(content, xml.OuterXml);
        }
    }
}
