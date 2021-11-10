// Copyright (c) Sbn.
// See LICENSE for more details.

using NUnit.Framework;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Infrastructure.Persistence.Mappers;
using Sbn.Cms.Tests.UnitTests.TestHelpers;
using Constants = Sbn.Cms.Core.Constants;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Infrastructure.Persistence.Mappers
{
    [TestFixture]
    public class ContentMapperTest
    {
        [Test]
        public void Can_Map_Id_Property()
        {
            var column = new ContentMapper(TestHelper.GetMockSqlContext(), TestHelper.CreateMaps()).Map(nameof(Content.Id));
            Assert.That(column, Is.EqualTo($"[{Constants.DatabaseSchema.Tables.Node}].[id]"));
        }

        [Test]
        public void Can_Map_Trashed_Property()
        {
            var column = new ContentMapper(TestHelper.GetMockSqlContext(), TestHelper.CreateMaps()).Map(nameof(Content.Trashed));
            Assert.That(column, Is.EqualTo($"[{Constants.DatabaseSchema.Tables.Node}].[trashed]"));
        }

        [Test]
        public void Can_Map_Published_Property()
        {
            var column = new ContentMapper(TestHelper.GetMockSqlContext(), TestHelper.CreateMaps()).Map(nameof(Content.Published));
            Assert.That(column, Is.EqualTo($"[{Constants.DatabaseSchema.Tables.Document}].[published]"));
        }

        [Test]
        public void Can_Map_Version_Property()
        {
            var column = new ContentMapper(TestHelper.GetMockSqlContext(), TestHelper.CreateMaps()).Map(nameof(Content.VersionId));
            Assert.That(column, Is.EqualTo($"[{Constants.DatabaseSchema.Tables.ContentVersion}].[id]"));
        }
    }
}
