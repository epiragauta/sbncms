// Copyright (c) Sbn.
// See LICENSE for more details.

using NUnit.Framework;
using Sbn.Cms.Infrastructure.Persistence.Mappers;
using Sbn.Cms.Tests.UnitTests.TestHelpers;
using Constants = Sbn.Cms.Core.Constants;
using MediaModel = Sbn.Cms.Core.Models.Media;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Infrastructure.Persistence.Mappers
{
    [TestFixture]
    public class MediaMapperTest
    {
        [Test]
        public void Can_Map_Id_Property()
        {
            var column = new MediaMapper(TestHelper.GetMockSqlContext(), TestHelper.CreateMaps()).Map(nameof(MediaModel.Id));
            Assert.That(column, Is.EqualTo($"[{Constants.DatabaseSchema.Tables.Node}].[id]"));
        }

        [Test]
        public void Can_Map_Trashed_Property()
        {
            var column = new MediaMapper(TestHelper.GetMockSqlContext(), TestHelper.CreateMaps()).Map(nameof(MediaModel.Trashed));
            Assert.That(column, Is.EqualTo($"[{Constants.DatabaseSchema.Tables.Node}].[trashed]"));
        }

        [Test]
        public void Can_Map_UpdateDate_Property()
        {
            var column = new MediaMapper(TestHelper.GetMockSqlContext(), TestHelper.CreateMaps()).Map(nameof(MediaModel.UpdateDate));
            Assert.That(column, Is.EqualTo($"[{Constants.DatabaseSchema.Tables.ContentVersion}].[versionDate]"));
        }

        [Test]
        public void Can_Map_Version_Property()
        {
            var column = new MediaMapper(TestHelper.GetMockSqlContext(), TestHelper.CreateMaps()).Map(nameof(MediaModel.VersionId));
            Assert.That(column, Is.EqualTo($"[{Constants.DatabaseSchema.Tables.ContentVersion}].[id]"));
        }
    }
}
