// Copyright (c) Sbn.
// See LICENSE for more details.

using NUnit.Framework;
using Sbn.Cms.Infrastructure.Persistence.Mappers;
using Sbn.Cms.Tests.UnitTests.TestHelpers;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Infrastructure.Persistence.Mappers
{
    [TestFixture]
    public class RelationTypeMapperTest
    {
        [Test]
        public void Can_Map_Id_Property()
        {
            // Act
            string column = new RelationTypeMapper(TestHelper.GetMockSqlContext(), TestHelper.CreateMaps()).Map("Id");

            // Assert
            Assert.That(column, Is.EqualTo("[sbnRelationType].[id]"));
        }

        [Test]
        public void Can_Map_Alias_Property()
        {
            // Act
            string column = new RelationTypeMapper(TestHelper.GetMockSqlContext(), TestHelper.CreateMaps()).Map("Alias");

            // Assert
            Assert.That(column, Is.EqualTo("[sbnRelationType].[alias]"));
        }

        [Test]
        public void Can_Map_ChildObjectType_Property()
        {
            // Act
            string column = new RelationTypeMapper(TestHelper.GetMockSqlContext(), TestHelper.CreateMaps()).Map("ChildObjectType");

            // Assert
            Assert.That(column, Is.EqualTo("[sbnRelationType].[childObjectType]"));
        }

        [Test]
        public void Can_Map_IsBidirectional_Property()
        {
            // Act
            string column = new RelationTypeMapper(TestHelper.GetMockSqlContext(), TestHelper.CreateMaps()).Map("IsBidirectional");

            // Assert
            Assert.That(column, Is.EqualTo("[sbnRelationType].[dual]"));
        }
    }
}
