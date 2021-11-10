using System;
using System.Collections.Generic;
using Sbn.Cms.Core.Composing;

namespace Sbn.Cms.Core.Trees
{
    /// <summary>
    /// Represents the collection of section trees.
    /// </summary>
    public class TreeCollection : BuilderCollectionBase<Tree>
    {

        public TreeCollection(Func<IEnumerable<Tree>> items) : base(items)
        {
        }
    }
}
