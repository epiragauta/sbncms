// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using NUnit.Framework;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Tests.Common.Builders;
using Sbn.Cms.Tests.Common.Builders.Extensions;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Tests.Common.Builders
{
    [TestFixture]
    public class DataTypeBuilderTests
    {
        [Test]
        public void Is_Built_Correctly()
        {
            // Arrange
            const int testId = 3123;
            const string testEditorAlias = "testAlias";
            const string testEditorName = "Test Name";

            var builder = new DataTypeBuilder();

            // Act
            DataType dataType = builder
                .WithId(testId)
                .AddEditor()
                    .WithAlias(testEditorAlias)
                    .WithName(testEditorName)
                    .WithDefaultConfiguration(new Dictionary<string, object>
                        {
                            { "value1", "value1" },
                            { "value2", "value2" },
                        })
                    .Done()
                .Build();

            // Assert
            Assert.AreEqual(testId, dataType.Id);
            Assert.AreEqual(testEditorAlias, dataType.Editor.Alias);
            Assert.AreEqual(testEditorName, dataType.Editor.Name);
            Assert.AreEqual(2, dataType.Editor.DefaultConfiguration.Count);
            Assert.AreEqual("value1", dataType.Editor.DefaultConfiguration["value1"]);
        }
    }
}
