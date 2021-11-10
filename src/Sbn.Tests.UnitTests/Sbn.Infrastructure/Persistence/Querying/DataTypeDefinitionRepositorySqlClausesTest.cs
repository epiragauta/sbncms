// Copyright (c) Sbn.
// See LICENSE for more details.

using System;
using System.Diagnostics;
using NPoco;
using NUnit.Framework;
using Sbn.Cms.Infrastructure.Persistence.Dtos;
using Sbn.Cms.Tests.UnitTests.TestHelpers;
using Sbn.Extensions;
using Constants = Sbn.Cms.Core.Constants;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Infrastructure.Persistence.Querying
{
    [TestFixture]
    public class DataTypeDefinitionRepositorySqlClausesTest : BaseUsingSqlSyntax
    {
        [Test]
        public void Can_Verify_Base_Clause()
        {
            var nodeObjectTypeId = Constants.ObjectTypes.DataType;

            var expected = new Sql();
            expected.Select("*")
                .From($"[{Constants.DatabaseSchema.Tables.DataType}]")
                .InnerJoin("[sbnNode]").On($"[{Constants.DatabaseSchema.Tables.DataType}].[nodeId] = [sbnNode].[id]")
                .Where("([sbnNode].[nodeObjectType] = @0)", new Guid("30a2a501-1978-4ddb-a57b-f7efed43ba3c"));

            var sql = Sql();
            sql.SelectAll()
               .From<DataTypeDto>()
               .InnerJoin<NodeDto>()
               .On<DataTypeDto, NodeDto>(left => left.NodeId, right => right.NodeId)
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
