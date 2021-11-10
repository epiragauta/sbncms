using System.Linq;
using NUnit.Framework;
using Sbn.Cms.Core.Trees;
using Sbn.Cms.Web.BackOffice.Trees;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Web.BackOffice.CollectionBuilders
{
    public class TreeCollectionBuilderTests
    {
        [Test]
        public void Adding_Tree_To_Collection_Builder()
        {
            var collectionBuilder = new TreeCollectionBuilder();
            var treeDefinition = new Tree(0, "test", "test", "test", "test", TreeUse.Main, typeof(LanguageTreeController), false);

            collectionBuilder.AddTree(treeDefinition);
            var collection = collectionBuilder.CreateCollection(null);

            Assert.AreEqual(1, collection.Count);
            Assert.AreEqual(treeDefinition, collection.FirstOrDefault());
        }

        [Test]
        public void Remove_Tree_From_Collection_Builder()
        {
            var collectionBuilder = new TreeCollectionBuilder();
            var treeDefinition = new Tree(0, "test", "test", "test", "test", TreeUse.Main, typeof(LanguageTreeController), false);

            collectionBuilder.AddTree(treeDefinition);
            collectionBuilder.RemoveTreeController<LanguageTreeController>();
            var collection = collectionBuilder.CreateCollection(null);

            Assert.AreEqual(0, collection.Count);
        }
    }
}
