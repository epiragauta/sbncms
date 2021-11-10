// Copyright (c) Sbn.
// See LICENSE for more details.

using NUnit.Framework;
using Sbn.Cms.Infrastructure.Persistence.Mappers;
using Sbn.Cms.Tests.UnitTests.TestHelpers;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Infrastructure.Persistence.Mappers
{
    [TestFixture]
    public class PropertyGroupMapperTest
    {
        [Test]
        public void Can_Map_Id_Property()
        {
            // Act
            string column = new PropertyGroupMapper(TestHelper.GetMockSqlContext(), TestHelper.CreateMaps()).Map("Id");

            // Assert
            Assert.That(column, Is.EqualTo("[cmsPropertyTypeGroup].[id]"));
        }

        [Test]
        public void Can_Map_SortOrder_Property()
        {
            // Act
            string column = new PropertyGroupMapper(TestHelper.GetMockSqlContext(), TestHelper.CreateMaps()).Map("SortOrder");

            // Assert
            Assert.That(column, Is.EqualTo("[cmsPropertyTypeGroup].[sortorder]"));
        }

        [Test]
        public void Can_Map_Name_Property()
        {
            // Act
            string column = new PropertyGroupMapper(TestHelper.GetMockSqlContext(), TestHelper.CreateMaps()).Map("Name");

            // Assert
            Assert.That(column, Is.EqualTo("[cmsPropertyTypeGroup].[text]"));
        }
    }
}
