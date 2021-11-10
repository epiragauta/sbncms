// Copyright (c) Sbn.
// See LICENSE for more details.

using System;
using System.Diagnostics;
using NPoco;
using NUnit.Framework;
using Sbn.Cms.Infrastructure.Persistence;
using Sbn.Cms.Infrastructure.Persistence.Dtos;
using Sbn.Cms.Tests.UnitTests.TestHelpers;
using Sbn.Extensions;
using Constants = Sbn.Cms.Core.Constants;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Infrastructure.Persistence.Querying
{
    [TestFixture]
    public class MediaRepositorySqlClausesTest : BaseUsingSqlSyntax
    {
        [Test]
        public void Can_Verify_Base_Clause()
        {
            Guid nodeObjectTypeId = Constants.ObjectTypes.Media;

            var expected = new Sql();
            expected.Select("*")
                .From($"[{Constants.DatabaseSchema.Tables.ContentVersion}]")
                .InnerJoin($"[{Constants.DatabaseSchema.Tables.Content}]").On($"[{Constants.DatabaseSchema.Tables.ContentVersion}].[nodeId] = [{Constants.DatabaseSchema.Tables.Content}].[nodeId]")
                .InnerJoin("[sbnNode]").On($"[{Constants.DatabaseSchema.Tables.Content}].[nodeId] = [sbnNode].[id]")
                .Where("([sbnNode].[nodeObjectType] = @0)", new Guid("b796f64c-1f99-4ffb-b886-4bf4bc011a9c"));

            Sql<ISqlContext> sql = Sql();
            sql.SelectAll()
                .From<ContentVersionDto>()
                .InnerJoin<ContentDto>()
                .On<ContentVersionDto, ContentDto>(left => left.NodeId, right => right.NodeId)
                .InnerJoin<NodeDto>()
                .On<ContentDto, NodeDto>(left => left.NodeId, right => right.NodeId)
                .Where<NodeDto>(x => x.NodeObjectType == nodeObjectTypeId);

            Assert.That(sql.SQL, Is.EqualTo(expected.SQL));

            Assert.AreEqual(expected.Arguments.Length, sql.Arguments.Length);
            for (int i = 0; i < expected.Arguments.Length; i++)
            {
                Assert.AreEqual(expected.Arguments[i], sql.Arguments[i]);
            }

            Debug.Print(sql.SQL);
        }
    }
}
