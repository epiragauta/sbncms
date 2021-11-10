using System;
using System.Collections.Generic;
using System.Linq;
using Examine;
using Examine.Lucene;
using Examine.Lucene.Directories;
using Examine.Lucene.Providers;
using Lucene.Net.Store;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Sbn.Cms.Core.Models.PublishedContent;
using Sbn.Cms.Core.PublishedCache;
using Sbn.Cms.Infrastructure;
using Sbn.Cms.Infrastructure.Examine;
using Sbn.Cms.Tests.Common.Testing;

namespace Sbn.Cms.Tests.Integration.Sbn.Examine.Lucene.SbnExamine
{
    [TestFixture]
    [SbnTest(Database = SbnTestOptions.Database.None)]
    public class PublishedContentQueryTests : ExamineBaseTest
    {

        private class TestIndex : LuceneIndex, ISbnIndex
        {
            private readonly string[] _fieldNames;

            public TestIndex(ILoggerFactory loggerFactory, string name, Directory luceneDirectory, string[] fieldNames)
                : base(
                      loggerFactory,
                      name,
                      IndexInitializer.GetOptions(name, new LuceneDirectoryIndexOptions
                      {
                          DirectoryFactory = new GenericDirectoryFactory(s => luceneDirectory)
                      }))
            {
                _fieldNames = fieldNames;
            }

            public bool EnableDefaultEventHandler => throw new NotImplementedException();
            public bool PublishedValuesOnly => throw new NotImplementedException();
            public IEnumerable<string> GetFields() => _fieldNames;
        }

        private TestIndex CreateTestIndex(Directory luceneDirectory, string[] fieldNames)
        {
            var index = new TestIndex(LoggerFactory, "TestIndex", luceneDirectory, fieldNames);

            using (index.WithThreadingMode(IndexThreadingMode.Synchronous))
            {
                //populate with some test data
                index.IndexItem(new ValueSet("1", "content", new Dictionary<string, object>
                {
                    [fieldNames[0]] = "Hello world, there are products here",
                    [SbnExamineFieldNames.VariesByCultureFieldName] = "n"
                }));
                index.IndexItem(new ValueSet("2", "content", new Dictionary<string, object>
                {
                    [fieldNames[1]] = "Hello world, there are products here",
                    [SbnExamineFieldNames.VariesByCultureFieldName] = "y"
                }));
                index.IndexItem(new ValueSet("3", "content", new Dictionary<string, object>
                {
                    [fieldNames[2]] = "Hello world, there are products here",
                    [SbnExamineFieldNames.VariesByCultureFieldName] = "y"
                }));
            }

            return index;
        }

        private PublishedContentQuery CreatePublishedContentQuery(IIndex indexer)
        {
            var examineManager = new Mock<IExamineManager>();
            IIndex outarg = indexer;
            examineManager.Setup(x => x.TryGetIndex("TestIndex", out outarg)).Returns(true);

            var contentCache = new Mock<IPublishedContentCache>();
            contentCache.Setup(x => x.GetById(It.IsAny<int>())).Returns((int intId) => Mock.Of<IPublishedContent>(x => x.Id == intId));
            var snapshot = Mock.Of<IPublishedSnapshot>(x => x.Content == contentCache.Object);
            var variationContext = new VariationContext();
            var variationContextAccessor = Mock.Of<IVariationContextAccessor>(x => x.VariationContext == variationContext);

            return new PublishedContentQuery(snapshot, variationContextAccessor, examineManager.Object);
        }

        [TestCase("fr-fr", ExpectedResult = "1, 3", Description = "Search Culture: fr-fr. Must return both fr-fr and invariant results")]
        [TestCase("en-us", ExpectedResult = "1, 2", Description = "Search Culture: en-us. Must return both en-us and invariant results")]
        [TestCase("*", ExpectedResult = "1, 2, 3", Description = "Search Culture: *. Must return all cultures and all invariant results")]
        [TestCase(null, ExpectedResult = "1", Description = "Search Culture: null. Must return only invariant results")]
        public string Search(string culture)
        {
            using (var luceneDir = new RandomIdRAMDirectory())
            {
                var fieldNames = new[] { "title", "title_en-us", "title_fr-fr" };
                using (var indexer = CreateTestIndex(luceneDir, fieldNames))
                {
                    var pcq = CreatePublishedContentQuery(indexer);

                    var results = pcq.Search("Products", culture, "TestIndex");

                    var ids = results.Select(x => x.Content.Id).ToArray();

                    return string.Join(", ", ids);
                }
            }
        }
    }
}
