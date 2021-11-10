// Copyright (c) Sbn.
// See LICENSE for more details.

using NUnit.Framework;
using Sbn.Cms.Infrastructure.Persistence.Mappers;
using Sbn.Cms.Tests.UnitTests.TestHelpers;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Infrastructure.Persistence.Mappers
{
    [TestFixture]
    public class ContentTypeMapperTest
    {
        [Test]
        public void Can_Map_Id_Property()
        {
            // Act
            string column = new ContentTypeMapper(TestHelper.GetMockSqlContext(), TestHelper.CreateMaps()).Map("Id");

            // Assert
            Assert.That(column, Is.EqualTo("[sbnNode].[id]"));
        }

        [Test]
        public void Can_Map_Name_Property()
        {
            // Act
            string column = new ContentTypeMapper(TestHelper.GetMockSqlContext(), TestHelper.CreateMaps()).Map("Name");

            // Assert
            Assert.That(column, Is.EqualTo("[sbnNode].[text]"));
        }

        [Test]
        public void Can_Map_Thumbnail_Property()
        {
            // Act
            string column = new ContentTypeMapper(TestHelper.GetMockSqlContext(), TestHelper.CreateMaps()).Map("Thumbnail");

            // Assert
            Assert.That(column, Is.EqualTo("[cmsContentType].[thumbnail]"));
        }

        [Test]
        public void Can_Map_Description_Property()
        {
            // Act
            string column = new ContentTypeMapper(TestHelper.GetMockSqlContext(), TestHelper.CreateMaps()).Map("Description");

            // Assert
            Assert.That(column, Is.EqualTo("[cmsContentType].[description]"));
        }
    }
}
