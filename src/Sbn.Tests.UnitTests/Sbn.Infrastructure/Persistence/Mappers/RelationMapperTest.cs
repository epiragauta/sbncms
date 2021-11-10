// Copyright (c) Sbn.
// See LICENSE for more details.

using NUnit.Framework;
using Sbn.Cms.Infrastructure.Persistence.Mappers;
using Sbn.Cms.Tests.UnitTests.TestHelpers;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Infrastructure.Persistence.Mappers
{
    [TestFixture]
    public class RelationMapperTest
    {
        [Test]
        public void Can_Map_Id_Property()
        {
            // Act
            string column = new RelationMapper(TestHelper.GetMockSqlContext(), TestHelper.CreateMaps()).Map("Id");

            // Assert
            Assert.That(column, Is.EqualTo("[sbnRelation].[id]"));
        }

        [Test]
        public void Can_Map_ChildId_Property()
        {
            // Act
            string column = new RelationMapper(TestHelper.GetMockSqlContext(), TestHelper.CreateMaps()).Map("ChildId");

            // Assert
            Assert.That(column, Is.EqualTo("[sbnRelation].[childId]"));
        }

        [Test]
        public void Can_Map_Datetime_Property()
        {
            // Act
            string column = new RelationMapper(TestHelper.GetMockSqlContext(), TestHelper.CreateMaps()).Map("CreateDate");

            // Assert
            Assert.That(column, Is.EqualTo("[sbnRelation].[datetime]"));
        }

        [Test]
        public void Can_Map_Comment_Property()
        {
            // Act
            string column = new RelationMapper(TestHelper.GetMockSqlContext(), TestHelper.CreateMaps()).Map("Comment");

            // Assert
            Assert.That(column, Is.EqualTo("[sbnRelation].[comment]"));
        }

        [Test]
        public void Can_Map_RelationType_Property()
        {
            // Act
            string column = new RelationMapper(TestHelper.GetMockSqlContext(), TestHelper.CreateMaps()).Map("RelationTypeId");

            // Assert
            Assert.That(column, Is.EqualTo("[sbnRelation].[relType]"));
        }
    }
}
