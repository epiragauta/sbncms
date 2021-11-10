// Copyright (c) Sbn.
// See LICENSE for more details.

using System;
using NUnit.Framework;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Tests.Common.Builders;
using Sbn.Cms.Tests.Common.Builders.Extensions;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Tests.Common.Builders
{
    [TestFixture]
    public class PropertyBuilderTests
    {
        [Test]
        public void Is_Built_Correctly()
        {
            // Arrange
            const int testId = 4;
            var testKey = Guid.NewGuid();
            DateTime testCreateDate = DateTime.Now.AddHours(-1);
            DateTime testUpdateDate = DateTime.Now;
            var testPropertyTypeId = 3;

            var builder = new PropertyBuilder();

            // Act
            IProperty property = builder
                .WithId(testId)
                .WithKey(testKey)
                .WithCreateDate(testCreateDate)
                .WithUpdateDate(testUpdateDate)
                .AddPropertyType()
                    .WithId(testPropertyTypeId)
                    .Done()
                .Build();

            // Assert
            Assert.AreEqual(testId, property.Id);
            Assert.AreEqual(testCreateDate, property.CreateDate);
            Assert.AreEqual(testUpdateDate, property.UpdateDate);
            Assert.AreEqual(testKey, property.Key);
            Assert.AreEqual(testPropertyTypeId, property.PropertyType.Id);
        }
    }
}
