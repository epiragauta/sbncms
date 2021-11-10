using System;
using System.Collections.Generic;
using System.Linq;
using Examine;
using Examine.Lucene.Providers;
using Examine.Search;
using Moq;
using NUnit.Framework;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Persistence.Querying;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Infrastructure.Examine;
using Sbn.Cms.Tests.Common.Testing;
using Sbn.Extensions;

namespace Sbn.Cms.Tests.Integration.Sbn.Examine.Lucene.SbnExamine
{
    [TestFixture]
    [SbnTest(Database = SbnTestOptions.Database.NewSchemaPerTest, Logger = SbnTestOptions.Logger.Console)]
    public class SearchTests : ExamineBaseTest
    {

        [Test]
        public void Test_Sort_Order_Sorting()
        {
            long totalRecs;
            var demoData = new ExamineDemoDataContentService(TestFiles.sbn_sort);
            var allRecs = demoData.GetLatestContentByXPath("//*[@isDoc]")
                .Root
                .Elements()
                .Select(x => Mock.Of<IContent>(
                    m =>
                        m.Id == (int)x.Attribute("id") &&
                        m.ParentId == (int)x.Attribute("parentID") &&
                        m.Level == (int)x.Attribute("level") &&
                        m.CreatorId == 0 &&
                        m.SortOrder == (int)x.Attribute("sortOrder") &&
                        m.CreateDate == (DateTime)x.Attribute("createDate") &&
                        m.UpdateDate == (DateTime)x.Attribute("updateDate") &&
                        m.Name == (string)x.Attribute(SbnExamineFieldNames.NodeNameFieldName) &&
                        m.GetCultureName(It.IsAny<string>()) == (string)x.Attribute(SbnExamineFieldNames.NodeNameFieldName) &&
                        m.Path == (string)x.Attribute("path") &&
                        m.Properties == new PropertyCollection() &&
                        m.Published == true &&
                        m.ContentType == Mock.Of<ISimpleContentType>(mt =>
                            mt.Icon == "test" &&
                            mt.Alias == x.Name.LocalName &&
                            mt.Id == (int)x.Attribute("nodeType"))))
                .ToArray();
            var contentService = Mock.Of<IContentService>(
                x => x.GetPagedDescendants(
                    It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>(), out totalRecs, It.IsAny<IQuery<IContent>>(), It.IsAny<Ordering>())
                    ==
                    allRecs);

            using (GetSynchronousContentIndex(false, out SbnContentIndex index, out ContentIndexPopulator contentRebuilder, out _, null, contentService))
            {
                index.CreateIndex();
                contentRebuilder.Populate(index);

                var searcher = index.Searcher;

                Assert.Greater(searcher.CreateQuery().All().Execute().TotalItemCount, 0);

                var numberSortedCriteria = searcher.CreateQuery()
                    .ParentId(1148)
                    .OrderBy(new SortableField("sortOrder", SortType.Int));
                var numberSortedResult = numberSortedCriteria.Execute();

                var stringSortedCriteria = searcher.CreateQuery()
                    .ParentId(1148)
                    .OrderBy(new SortableField("sortOrder"));//will default to string
                var stringSortedResult = stringSortedCriteria.Execute();

                Assert.AreEqual(12, numberSortedResult.TotalItemCount);
                Assert.AreEqual(12, stringSortedResult.TotalItemCount);

                Assert.IsTrue(IsSortedByNumber(numberSortedResult));
                Assert.IsFalse(IsSortedByNumber(stringSortedResult));
            }
        }

        private bool IsSortedByNumber(IEnumerable<ISearchResult> results)
        {
            var currentSort = 0;
            foreach (var searchResult in results)
            {
                var sort = int.Parse(searchResult.Values["sortOrder"]);
                if (currentSort >= sort)
                {
                    return false;
                }
                currentSort = sort;
            }
            return true;
        }

    }
}
