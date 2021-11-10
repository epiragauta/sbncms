// Copyright (c) Sbn.
// See LICENSE for more details.

using NUnit.Framework;
using Sbn.Cms.Infrastructure.Persistence.Mappers;
using Sbn.Cms.Tests.UnitTests.TestHelpers;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Infrastructure.Persistence.Mappers
{
    [TestFixture]
    public class LanguageMapperTest
    {
        [Test]
        public void Can_Map_Id_Property()
        {
            // Act
            string column = new LanguageMapper(TestHelper.GetMockSqlContext(), TestHelper.CreateMaps()).Map("Id");

            // Assert
            Assert.That(column, Is.EqualTo("[sbnLanguage].[id]"));
        }

        [Test]
        public void Can_Map_IsoCode_Property()
        {
            // Act
            string column = new LanguageMapper(TestHelper.GetMockSqlContext(), TestHelper.CreateMaps()).Map("IsoCode");

            // Assert
            Assert.That(column, Is.EqualTo("[sbnLanguage].[languageISOCode]"));
        }

        [Test]
        public void Can_Map_CultureName_Property()
        {
            // Act
            string column = new LanguageMapper(TestHelper.GetMockSqlContext(), TestHelper.CreateMaps()).Map("CultureName");

            // Assert
            Assert.That(column, Is.EqualTo("[sbnLanguage].[languageCultureName]"));
        }
    }
}
