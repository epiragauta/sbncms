// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Diagnostics;
using NPoco;
using NUnit.Framework;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Persistence.Querying;
using Sbn.Cms.Infrastructure.Persistence;
using Sbn.Cms.Infrastructure.Persistence.Dtos;
using Sbn.Cms.Infrastructure.Persistence.Querying;
using Sbn.Cms.Tests.UnitTests.TestHelpers;
using Sbn.Extensions;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Infrastructure.Persistence.Querying
{
    [TestFixture]
    public class QueryBuilderTests : BaseUsingSqlSyntax
    {
        [Test]
        public void Can_Build_StartsWith_Query_For_IContent()
        {
            // Arrange
            Sql<ISqlContext> sql = Sql();
            sql.SelectAll();
            sql.From("sbnNode");

            IQuery<IContent> query = new Query<IContent>(SqlContext).Where(x => x.Path.StartsWith("-1"));

            // Act
            var translator = new SqlTranslator<IContent>(sql, query);
            Sql<ISqlContext> result = translator.Translate();
            string strResult = result.SQL;

            string expectedResult = "SELECT *\nFROM sbnNode\nWHERE (upper([sbnNode].[path]) LIKE upper(@0))";

            // Assert
            Assert.That(strResult, Is.Not.Empty);
            Assert.That(strResult, Is.EqualTo(expectedResult));

            Assert.AreEqual(1, result.Arguments.Length);
            Assert.AreEqual("-1%", sql.Arguments[0]);

            Debug.Print(strResult);
        }

        [Test]
        public void Can_Build_ParentId_Query_For_IContent()
        {
            // Arrange
            Sql<ISqlContext> sql = Sql();
            sql.SelectAll();
            sql.From("sbnNode");

            IQuery<IContent> query = new Query<IContent>(SqlContext).Where(x => x.ParentId == -1);

            // Act
            var translator = new SqlTranslator<IContent>(sql, query);
            Sql<ISqlContext> result = translator.Translate();
            string strResult = result.SQL;

            string expectedResult = "SELECT *\nFROM sbnNode\nWHERE (([sbnNode].[parentId] = @0))";

            // Assert
            Assert.That(strResult, Is.Not.Empty);
            Assert.That(strResult, Is.EqualTo(expectedResult));

            Assert.AreEqual(1, result.Arguments.Length);
            Assert.AreEqual(-1, sql.Arguments[0]);

            Debug.Print(strResult);
        }

        [Test]
        public void Can_Build_ContentTypeAlias_Query_For_IContentType()
        {
            // Arrange
            Sql<ISqlContext> sql = Sql();
            sql.SelectAll();
            sql.From("sbnNode");

            IQuery<IContentType> query = new Query<IContentType>(SqlContext).Where(x => x.Alias == "umbTextpage");

            // Act
            var translator = new SqlTranslator<IContentType>(sql, query);
            Sql<ISqlContext> result = translator.Translate();
            string strResult = result.SQL;

            string expectedResult = "SELECT *\nFROM sbnNode\nWHERE (([cmsContentType].[alias] = @0))";

            // Assert
            Assert.That(strResult, Is.Not.Empty);
            Assert.That(strResult, Is.EqualTo(expectedResult));
            Assert.AreEqual(1, result.Arguments.Length);
            Assert.AreEqual("umbTextpage", sql.Arguments[0]);

            Debug.Print(strResult);
        }

        [Test]
        public void Can_Build_PublishedDescendants_Query_For_IContent()
        {
            const string path = "-1,1046,1076,1089";
            const int id = 1046;

            Sql<ISqlContext> sql = Sql();
            sql.SelectAll()
                .From<DocumentDto>(); // the actual SELECT really does not matter

            IQuery<IContent> query = SqlContext.Query<IContent>().Where(x => x.Path.StartsWith(path) && x.Id != id && x.Published && x.Trashed == false);

            var translator = new SqlTranslator<IContent>(sql, query);
            Sql<ISqlContext> result = translator.Translate();

            Assert.AreEqual("-1,1046,1076,1089%", result.Arguments[0]);
            Assert.AreEqual(1046, result.Arguments[1]);
            Assert.AreEqual(true, result.Arguments[2]);
            Assert.AreEqual(false, result.Arguments[3]);
        }
    }
}
