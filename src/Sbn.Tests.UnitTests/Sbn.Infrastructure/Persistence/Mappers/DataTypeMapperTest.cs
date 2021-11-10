// Copyright (c) Sbn.
// See LICENSE for more details.

using NUnit.Framework;
using Sbn.Cms.Infrastructure.Persistence.Mappers;
using Sbn.Cms.Tests.UnitTests.TestHelpers;
using Constants = Sbn.Cms.Core.Constants;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Infrastructure.Persistence.Mappers
{
    [TestFixture]
    public class DataTypeMapperTest
    {
        [Test]
        public void Can_Map_Id_Property()
        {
            // Act
            string column = new DataTypeMapper(TestHelper.GetMockSqlContext(), TestHelper.CreateMaps()).Map("Id");

            // Assert
            Assert.That(column, Is.EqualTo("[sbnNode].[id]"));
        }

        [Test]
        public void Can_Map_Key_Property()
        {
            // Act
            string column = new DataTypeMapper(TestHelper.GetMockSqlContext(), TestHelper.CreateMaps()).Map("Key");

            // Assert
            Assert.That(column, Is.EqualTo("[sbnNode].[uniqueId]"));
        }

        [Test]
        public void Can_Map_DatabaseType_Property()
        {
            // Act
            string column = new DataTypeMapper(TestHelper.GetMockSqlContext(), TestHelper.CreateMaps()).Map("DatabaseType");

            // Assert
            Assert.That(column, Is.EqualTo($"[{Constants.DatabaseSchema.Tables.DataType}].[dbType]"));
        }

        [Test]
        public void Can_Map_PropertyEditorAlias_Property()
        {
            // Act
            string column = new DataTypeMapper(TestHelper.GetMockSqlContext(), TestHelper.CreateMaps()).Map("EditorAlias");

            // Assert
            Assert.That(column, Is.EqualTo($"[{Constants.DatabaseSchema.Tables.DataType}].[propertyEditorAlias]"));
        }
    }
}
