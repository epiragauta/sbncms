// Copyright (c) Sbn.
// See LICENSE for more details.

using NUnit.Framework;
using Sbn.Cms.Infrastructure.Persistence.Mappers;
using Sbn.Cms.Tests.UnitTests.TestHelpers;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Infrastructure.Persistence.Mappers
{
    [TestFixture]
    public class DictionaryMapperTest
    {
        [Test]
        public void Can_Map_Id_Property()
        {
            // Act
            string column = new DictionaryMapper(TestHelper.GetMockSqlContext(), TestHelper.CreateMaps()).Map("Id");

            // Assert
            Assert.That(column, Is.EqualTo("[cmsDictionary].[pk]"));
        }

        [Test]
        public void Can_Map_Key_Property()
        {
            // Act
            string column = new DictionaryMapper(TestHelper.GetMockSqlContext(), TestHelper.CreateMaps()).Map("Key");

            // Assert
            Assert.That(column, Is.EqualTo("[cmsDictionary].[id]"));
        }

        [Test]
        public void Can_Map_ItemKey_Property()
        {
            // Act
            string column = new DictionaryMapper(TestHelper.GetMockSqlContext(), TestHelper.CreateMaps()).Map("ItemKey");

            // Assert
            Assert.That(column, Is.EqualTo("[cmsDictionary].[key]"));
        }
    }
}
