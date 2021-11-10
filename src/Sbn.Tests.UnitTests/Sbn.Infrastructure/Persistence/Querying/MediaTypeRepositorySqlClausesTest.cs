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
    public class MediaTypeRepositorySqlClausesTest : BaseUsingSqlSyntax
    {
        [Test]
        public void Can_Verify_Base_Clause()
        {
            Guid nodeObjectTypeId = Constants.ObjectTypes.MediaType;

            var expected = new Sql();
            expected.Select("*")
                .From("[cmsContentType]")
                .InnerJoin("[sbnNode]").On("[cmsContentType].[nodeId] = [sbnNode].[id]")
                .Where("([sbnNode].[nodeObjectType] = @0)", new Guid("4ea4382b-2f5a-4c2b-9587-ae9b3cf3602e"));

            Sql<ISqlContext> sql = Sql();
            sql.SelectAll()
                .From<ContentTypeDto>()
                .InnerJoin<NodeDto>()
                .On<ContentTypeDto, NodeDto>(left => left.NodeId, right => right.NodeId)
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
